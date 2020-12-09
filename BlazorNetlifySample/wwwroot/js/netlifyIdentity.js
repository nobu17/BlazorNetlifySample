let dotNetObjRef;
export function init(dotNetObj) {
    if (!dotNetObj) {
        throw err("dotNetObj should be not null");
    }
    dotNetObjRef = dotNetObj;
    netlifyIdentity.on('login', user => dotNetObjRef.invokeMethod("OnLogin", user));
    netlifyIdentity.on('logout', () => dotNetObjRef.invokeMethod("OnLoggedOut"));
    netlifyIdentity.on('error', err => dotNetObjRef.invokeMethod("OnError", err));
}

export function getCurrentUser() {
    return netlifyIdentity.currentUser();
}

export function openDialog() {
    netlifyIdentity.open();
}

export function closeDialog() {
    netlifyIdentity.close();
}

export function logout() {
    netlifyIdentity.logout();
}