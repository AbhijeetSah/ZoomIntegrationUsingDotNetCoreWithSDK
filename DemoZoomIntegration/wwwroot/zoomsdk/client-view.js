ZoomMtg.setZoomJSLib('https://source.zoom.us/2.9.5/lib', '/av')

ZoomMtg.preLoadWasm()
ZoomMtg.prepareWebSDK()
// loads language files, also passes any error messages to the ui
ZoomMtg.i18n.load('en-US')
ZoomMtg.i18n.reload('en-US')

var leaveUrl = '/';
var sdkKey = 'MaDOA4PkoxzybFiwRzuoQtTIGIoLtrVymwR5'; //sdk key

function startMeeting(signature, meetingNumber, userName, userEmail, passWord, registrantToken) {
    debugger;
    document.getElementById('zmmtg-root').style.display = 'block';

    ZoomMtg.init({
        leaveUrl: leaveUrl,
        success: (success) => {
            console.log(success)
            ZoomMtg.join({
                signature: signature,
                sdkKey: sdkKey,
                meetingNumber: meetingNumber,
                userName: userName,
                userEmail: userEmail,
                passWord: passWord,
                tk: registrantToken,
                success: (success) => {
                    console.log(success)
                },
                error: (error) => {
                    console.log(error)
                },
            })
        },
        error: (error) => {
            console.log(error)
        }
    });
}
