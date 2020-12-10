let auth;
function initAuth() {
    auth = new GoTrue({
        APIUrl: 'https://eloquent-wozniak-b3a6b2.netlify.app/.netlify/identity',
        setCookie: true
    });
}

async function loginAsync(email, passowrd) {
    console.log("loginAsync")
    return await new Promise((resolve, reject) => {
        auth.login(email, passowrd, true)
            .then((response) => {
                const result = { accessToken: response.token.access_token, userId: email };
                resolve(result);
            })
            .catch((error) => {
                reject(error);
            });
    });
}

async function logoutAsync() {
    return await new Promise((resolve, reject) => {
        const user = auth.currentUser();
        if (user) {
            resolve(null);
            return;
        }
        user.logout()
            .then(response => {
                console.log("User logged out");
                resolve(response);
            })
            .catch(error => {
                console.log("Failed to logout user: %o", error);
                reject(error);
            });
    });
}

function getCurrentUser() {
    const user = auth.currentUser();
    if (user) {
        return { accessToken: user.token.access_token, userId: user.email }
    } else {
        return null;
    }
}
