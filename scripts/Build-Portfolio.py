from pathlib import Path
import re,html,textwrap,os,json
from urllib.parse import quote
from reportlab.platypus import BaseDocTemplate,PageTemplate,Frame,Paragraph,Spacer,PageBreak,Table,TableStyle,Image,KeepTogether,Preformatted,CondPageBreak
from reportlab.platypus.tableofcontents import TableOfContents
from reportlab.lib import colors
from reportlab.lib.styles import ParagraphStyle
from reportlab.lib.pagesizes import A4
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.graphics.shapes import Drawing,Rect,Line,String,Polygon
from PIL import Image as PILImage

ROOT=Path(__file__).resolve().parents[1];OUT=ROOT/'docs/HIT339_Group8_Portfolio.pdf'
fontdir=Path('/usr/share/fonts/truetype/dejavu')
files=[('Body','DejaVuSans.ttf'),('Bold','DejaVuSans-Bold.ttf'),('Code','DejaVuSansMono.ttf')]
if not fontdir.exists():
 fontdir=Path(os.environ.get('WINDIR','C:/Windows'))/'Fonts';files=[('Body','arial.ttf'),('Bold','arialbd.ttf'),('Code','consola.ttf')]
for n,f in files:pdfmetrics.registerFont(TTFont(n,str(fontdir/f)))
pdfmetrics.registerFontFamily('Body',normal='Body',bold='Bold',italic='Body',boldItalic='Bold')
W,H=A4;M=46;WIDTH=W-2*M;GREEN=colors.HexColor('#194D42');INK=colors.HexColor('#192D29');MUTED=colors.HexColor('#52645F');LIGHT=colors.HexColor('#EFF5F2');LINE=colors.HexColor('#D3DFD8')
styles={
 'text':ParagraphStyle('text',fontName='Body',fontSize=9.8,leading=14.1,spaceAfter=7.5,textColor=INK,allowWidows=0,allowOrphans=0),
 'cell':ParagraphStyle('cell',fontName='Body',fontSize=8.2,leading=11.4,textColor=INK,allowWidows=0,allowOrphans=0),
 'thead':ParagraphStyle('thead',fontName='Bold',fontSize=8.2,leading=11.4,textColor=GREEN),
 'h1':ParagraphStyle('h1',fontName='Bold',fontSize=22,leading=27,spaceAfter=15,textColor=GREEN,keepWithNext=True),
 'h2':ParagraphStyle('h2',fontName='Bold',fontSize=13,leading=18,spaceBefore=15,spaceAfter=7,textColor=GREEN,keepWithNext=True),
 'h3':ParagraphStyle('h3',fontName='Bold',fontSize=10.5,leading=14,spaceBefore=10,spaceAfter=5,textColor=INK,keepWithNext=True),
 'caption':ParagraphStyle('caption',fontName='Body',fontSize=8.3,leading=11.8,spaceBefore=7,spaceAfter=12,textColor=MUTED,allowWidows=0,allowOrphans=0),
 'code':ParagraphStyle('code',fontName='Code',fontSize=7.7,leading=10.8,backColor=LIGHT,borderPadding=7,spaceBefore=5,spaceAfter=10),
 'title':ParagraphStyle('title',fontName='Bold',fontSize=36,leading=44,spaceAfter=12,textColor=GREEN),
 'subtitle':ParagraphStyle('subtitle',fontName='Body',fontSize=14,leading=21,spaceAfter=12,textColor=MUTED)}
URL='https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub'
def inline(raw,source=None):
 for a,b in [('\ufeff',''),('\u2011','-'),('\u2013','-'),('\u2014',' - '),('\u2019',"'"),('\u2018',"'"),('\u201c','"'),('\u201d','"'),('\u2192',' to ')]:raw=raw.replace(a,b)
 links=[]
 def replace(m):
  label,target=m.groups()
  if not target.startswith(('http://','https://')):
   p=((source or ROOT/'docs')/target).resolve()
   try:rel=p.relative_to(ROOT).as_posix()
   except ValueError:rel=target
   target=URL+'/blob/integration/group8-release-16oct/'+quote(rel,safe='/')
  links.append('<link href="'+html.escape(target,quote=True)+'" color="#246451"><u>'+html.escape(label)+'</u></link>');return f'LINKTOKEN{len(links)-1}END'
 raw=re.sub(r'\[([^\]]+)\]\(([^)]+)\)',replace,raw);raw=html.escape(raw)
 raw=re.sub(r'`([^`]+)`',r'<font name="Code" size="8.2">\1</font>',raw);raw=re.sub(r'\*\*([^*]+)\*\*',r'<b>\1</b>',raw)
 for i,v in enumerate(links):raw=raw.replace(f'LINKTOKEN{i}END',v)
 return raw
def p(t,kind='text',source=None):return Paragraph(inline(t,source),styles[kind])
def table(rows,source):
 n=len(rows[0]);headers=' '.join(rows[0]).lower()
 if n==2:w=[.77,.23] if 'hour' in headers else [.34,.66]
 elif n==3:w=[.25,.38,.37]
 elif n==4:
  if rows[0][0]=='ID':w=[.09,.43,.16,.32]
  elif 'elapsed' in headers:w=[.13,.19,.16,.52]
  elif 'original estimate' in headers:w=[.41,.19,.20,.20]
  elif rows[0][0]=='Member':w=[.18,.15,.41,.26]
  else:w=[.15,.34,.20,.31]
 else:w=[1/n]*n
 if n==3 and ('before' in headers):w=[.52,.24,.24]
 if n==3 and ('marks' in headers):w=[.33,.13,.54]
 if n==3 and ('estimated session' in headers):w=[.22,.60,.18]
 if n==4 and rows[0][0]=='Stage':w=[.10,.25,.40,.25]
 if n==4 and 'deliverable or result' in headers:w=[.25,.20,.13,.42]
 data=[[p(c,'thead' if i==0 else 'cell',source) for c in r] for i,r in enumerate(rows)]
 pad=4.5 if n==2 and 'hour' in headers else 6
 t=Table(data,colWidths=[WIDTH*x for x in w],repeatRows=1,hAlign='LEFT');t.setStyle(TableStyle([('BACKGROUND',(0,0),(-1,0),LIGHT),('VALIGN',(0,0),(-1,-1),'TOP'),('LEFTPADDING',(0,0),(-1,-1),6),('RIGHTPADDING',(0,0),(-1,-1),6),('TOPPADDING',(0,0),(-1,-1),pad),('BOTTOMPADDING',(0,0),(-1,-1),pad),('LINEBELOW',(0,0),(-1,0),.7,LINE),('LINEBELOW',(0,1),(-1,-1),.35,LINE)]));return [t,Spacer(1,10)]
def arrow(d,x,y,a,b):
 import math
 d.add(Line(x,y,a,b,strokeColor=MUTED));angle=math.atan2(b-y,a-x);size=5
 d.add(Polygon([a,b,a-size*math.cos(angle-.5),b-size*math.sin(angle-.5),a-size*math.cos(angle+.5),b-size*math.sin(angle+.5)],fillColor=MUTED,strokeColor=MUTED))
def box(d,x,y,w,h,ls):
 d.add(Rect(x,y,w,h,rx=5,ry=5,fillColor=LIGHT,strokeColor=LINE))
 for i,t in enumerate(ls):d.add(String(x+w/2,y+h/2+(len(ls)-1)*6-i*12,t,fontName='Bold' if i==0 else 'Body',fontSize=8.1,fillColor=GREEN,textAnchor='middle'))
def architecture():
 d=Drawing(WIDTH,270);bw=WIDTH/3-8;xs=[0,WIDTH/3+2,2*WIDTH/3+4]
 for x,ls in zip(xs,[['Public catalogue','and kiosk'],['Staff desks','holds and transfers'],['External clients','key-protected API']]):box(d,x,218,bw,44,ls);arrow(d,x+bw/2,218,x+bw/2,187)
 box(d,42,128,WIDTH-84,58,['Shared services and eligibility','Identity | circulation | holds','one report definition | Darwin dates'])
 for x,ls in zip(xs,[['Admin CSV import','validated copies'],['SQL Server','inventory, loans, holds','branches and Identity'],['Manager reports','CSV/PDF audits']]):box(d,x,32,bw,56,ls)
 arrow(d,WIDTH/2,128,WIDTH/2,90);arrow(d,bw,60,xs[1],60);arrow(d,xs[1]+bw,60,xs[2],60)
 d.add(String(WIDTH/2,7,'Simulated Email/SMS events are persisted with the shared data.',fontName='Body',fontSize=8,fillColor=MUTED,textAnchor='middle'));return d
def holdstates():
 d=Drawing(WIDTH,190);bw=108;gap=(WIDTH-4*bw)/3
 for i,ls in enumerate([['Waiting','FIFO order'],['Allocated','repair/transfer'],['Ready','at pickup branch'],['Fulfilled','loan created']]):
  x=i*(bw+gap);box(d,x,115,bw,48,ls)
  if i<3:arrow(d,x+bw,139,x+bw+gap,139)
 box(d,WIDTH/2-65,24,130,42,['Cancelled','active request ends'])
 for i in range(3):arrow(d,i*(bw+gap)+bw/2,115,WIDTH/2,67)
 return d
toc=TableOfContents();toc.levelStyles=[ParagraphStyle('toc0',fontName='Bold',fontSize=10.1,leading=15,spaceBefore=8,textColor=GREEN,rightIndent=22),ParagraphStyle('toc1',fontName='Body',fontSize=9.3,leading=13,leftIndent=15,spaceBefore=4,textColor=INK)]
def parse(text,source):
 lines=text.splitlines();items=[];i=0
 while i<len(lines):
  s=lines[i].strip()
  if not s:i+=1;continue
  if s.startswith(':::include '):
   path=(source/s.split(' ',1)[1]).resolve();ls=path.read_text(encoding='utf-8-sig').splitlines()
   if ls and ls[0].startswith('# '):ls=ls[1:]
   items.extend(parse('\n'.join(ls),path.parent));i+=1;continue
  if s==':::toc':items.extend([PageBreak(),p('Contents','h1'),toc]);i+=1;continue
  if s==':::architecture':items.extend([architecture(),Spacer(1,4)]);i+=1;continue
  if s==':::holdstates':items.extend([holdstates(),Spacer(1,4)]);i+=1;continue
  if s.startswith(':::figure '):
   name,caption=s[10:].split('|',1);path=ROOT/name.strip()
   with PILImage.open(path) as im:w,h=im.size
   scale=min(WIDTH/w,550/h);items.extend([CondPageBreak(160),KeepTogether([Image(str(path),width=w*scale,height=h*scale,hAlign='CENTER'),p(caption.strip(),'caption')])]);i+=1;continue
  if s.startswith('```'):
   code=[];i+=1
   while i<len(lines) and not lines[i].strip().startswith('```'):
    code.extend(textwrap.wrap(lines[i],98,break_long_words=True,break_on_hyphens=False,replace_whitespace=False,drop_whitespace=False) or ['']);i+=1
   items.append(Preformatted('\n'.join(code),styles['code']));i+=1;continue
  if s.startswith('|'):
   rows=[]
   while i<len(lines) and lines[i].strip().startswith('|'):
    row=[x.strip() for x in lines[i].strip().strip('|').split('|')]
    if not all(re.fullmatch(r':?-+:?',x.replace(' ','')) for x in row):rows.append(row)
    i+=1
   items.extend(table(rows,source));continue
  h=re.match(r'^(#{1,6})\s+(.+)$',s)
  if h:
   level=len(h.group(1));title=h.group(2)
   if title=='My source history':items.append(CondPageBreak(320))
   if title=='Rubric coverage':items.append(CondPageBreak(470))
   if title=='My acknowledgement of assistance':items.append(PageBreak())
   if level==1 or (level==2 and re.match(r'6\.[2-4] ',title)):
    if items and not isinstance(items[-1],PageBreak):items.append(PageBreak())
   items.append(p(title,'h1' if level==1 else 'h2' if level==2 else 'h3',source));i+=1;continue
  block=[s];i+=1
  if not re.match(r'^(\d+\. |[-*] )',s):
   while i<len(lines) and lines[i].strip() and not re.match(r'^(#|\||:::|```|\d+\. |[-*] )',lines[i].strip()):block.append(lines[i].strip());i+=1
  items.append(p(' '.join(block),'text',source))
 return items
class Doc(BaseDocTemplate):
 def afterFlowable(self,f):
  if isinstance(f,Paragraph) and f.style.name in ('h1','h2'):
   t=f.getPlainText()
   if t!='Contents' and (f.style.name=='h1' or re.match(r'6\.[1-4] ',t)):
    level=0 if f.style.name=='h1' else 1;key='section-'+re.sub(r'[^A-Za-z0-9]+','-',t)
    self.canv.bookmarkPage(key);self.canv.addOutlineEntry(t,key,level=level,closed=False);self.notify('TOCEntry',(level,t,self.page,key))
def decorate(c,doc):
 c.saveState();c.setFont('Body',7.5);c.setFillColor(MUTED);c.setStrokeColor(LINE);c.setLineWidth(.6)
 if doc.page>1:c.drawString(M,H-26,'TopEndLibraryHub  |  Group 8 portfolio');c.drawRightString(W-M,H-26,'HIT339  |  16 October 2026');c.line(M,H-35,W-M,H-35)
 c.line(M,36,W-M,36);c.drawString(M,23,'Group 8  |  Source, decisions and actual outcomes');c.drawRightString(W-M,23,str(doc.page));c.restoreState()
story=[Spacer(1,32),p('TopEndLibraryHub','title'),p('Group Portfolio','subtitle'),p('HIT339 Distributed Development  |  Group 8'),p('16 October 2026'),Spacer(1,15)]
story+=table([['Group member','Student number'],['Kamalpreet','S389008'],['Harsimran Singh','S388232'],['Mehakpreet Kaur','S390303'],['Sandesh Bhattarai','S390535']],ROOT/'docs')
story+=[Spacer(1,14),p('A family can keep its place while an item is repaired, moved to the chosen branch and made ready for collection.','subtitle'),p('[Source repository]('+URL+') | [Project board](https://github.com/users/Kamal03102006/projects/1)'),p('Windows-tested revision: **b79faf3**. Source branch: integration/group8-release-16oct.'),p('Recorded results: 33 Windows tests and ten HTTPS API checks passed; actual imports and report/payment reconciliation verified. The separate VM installation result is recorded in section 8.')]
body=(ROOT/'docs/PORTFOLIO_CONTENT.md').read_text();body=':::toc'+body.split(':::toc',1)[1];story+=parse(body,ROOT/'docs')
doc=Doc(str(OUT),pagesize=A4,leftMargin=M,rightMargin=M,topMargin=52,bottomMargin=48,title='TopEndLibraryHub - HIT339 Group 8 Portfolio',author='HIT339 Group 8',subject='Project management, implementation, individual accounts and verification')
doc.addPageTemplates(PageTemplate(id='portfolio',frames=[Frame(M,48,WIDTH,H-100,leftPadding=0,rightPadding=0,topPadding=0,bottomPadding=0)],onPage=decorate));doc.multiBuild(story,maxPasses=5)
print(json.dumps({'pdf':str(OUT),'bytes':OUT.stat().st_size}))
