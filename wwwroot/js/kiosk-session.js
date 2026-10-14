(() => {
    const finishForm = document.querySelector('[data-kiosk-finish]');
    if (!finishForm) return;
    const idleLimit = 120000;
    let lastActivity = Date.now();
    let finishing = false;
    let sending = false;

    const finish = () => {
        if (finishing) return;
        finishing = true;
        finishForm.requestSubmit();
    };
    const recordActivity = () => {
        if (Date.now() - lastActivity >= idleLimit) { finish(); return; }
        lastActivity = Date.now();
    };
    for (const event of ['pointerdown', 'pointermove', 'keydown', 'touchstart', 'scroll'])
        document.addEventListener(event, recordActivity, { passive: true });
    window.addEventListener('pageshow', event => {
        if (event.persisted) window.location.reload();
    });
    document.addEventListener('visibilitychange', () => {
        if (document.visibilityState === 'visible' && Date.now() - lastActivity >= idleLimit) finish();
    });
    setInterval(() => { if (Date.now() - lastActivity >= idleLimit) finish(); }, 1000);
    // Only real recent interaction refreshes the server timeout; an idle tab does not.
    setInterval(async () => {
        if (finishing || sending || document.visibilityState !== 'visible' ||
            Date.now() - lastActivity > 30000) return;
        sending = true;
        try {
            const token = finishForm.querySelector('input[name="__RequestVerificationToken"]')?.value;
            const response = await fetch(finishForm.dataset.keepalive, {
                method: 'POST', credentials: 'same-origin',
                headers: { 'RequestVerificationToken': token ?? '' }
            });
            if (response.status === 401 || response.redirected) finish();
        } catch { /* Server access checks still enforce expiry if the connection fails. */ }
        finally { sending = false; }
    }, 25000);
})();
