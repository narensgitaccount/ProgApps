const sessionMarkerKey = "ade_session_active";

export function isNewBrowserSession() {
    if (sessionStorage.getItem(sessionMarkerKey)) {
        return false;
    }
    sessionStorage.setItem(sessionMarkerKey, "1");
    return true;
}

export function clearLocalStorageKeys(keys) {
    for (const key of keys) {
        localStorage.removeItem(key);
    }
}
