NSMobile.views.Login = Ext.extend(Ext.Panel, {
    fullscreen: true,
    baseCls: 'loginPane',
    id: 'login',
    html: [
		'<div id="loginPanel">',
        '<section class="partyStats loginForm inputForm">',
            '<div class="inputRow">',
                '<input type="text" placeholder="Username" id="username" onkeypress="return Ext.getCmp(\'login\').checkKeyPress(event)" />',
            '</div>',
			'<div class="inputRow">',
                '<input type="password" placeholder="Password" id="password" onkeypress="return Ext.getCmp(\'login\').checkKeyPress(event)" />',
            '</div>',
        '</section>',
		'<section class="formSubmit">',
		    '<input type="submit"  onclick="javascript:(function(){Ext.getCmp(\'login\').doLogin()})()" value="Sign In" />',
        '</section>',
		'</div>'
    ],
    doLogin: function () {
        var userBox = document.querySelector('#username');
        var passBox = document.querySelector('#password');
        var username = userBox.value;
        var password = passBox.value;
        userBox.blur();
        passBox.blur();
        if (username && password) {
            var loadMask = new Ext.LoadMask(Ext.getBody());
            loadMask.show();
            Ext.Ajax.request({
                url: serviceURI + '/Login',
                params: {
                    username: username,
                    password: password
                },
                method: 'GET',
                success: function (response, options) {
                    var responseObj = JSON.parse(response.responseText);
                    loadMask.hide();
                    if (responseObj.LoginSuccess) {
                        var storageData = getStorage('auth');
                        storageData['hash'] = responseObj.Hash;
                        storageData['currentuser'] = username;
                        setStorage('auth', storageData);

                        Ext.Ajax.extraParams = { hash: responseObj.Hash };

                        if (NSMobile.views.Viewport)
                            NSMobile.views.viewport = new NSMobile.views.Viewport();
                        else {
                            loadMask.show();
                            pageLoadCallback = function () {
                                yepnope({
                                    test: NSMobile.views.Viewport,
                                    nope: 'app/views/Viewport.js',
                                    complete: function () {
                                        loadMask.hide();
                                        NSMobile.views.viewport = new NSMobile.views.Viewport();
                                    }
                                });
                            };
                        }
                    }
                    else {
                        Ext.Msg.alert('Error', 'Couldn\'t log in. Please check your username and password.');
                    }
                },
                failure: function (reponse, options) {
                    loadMask.hide();
                    Ext.Msg.alert('Error', 'Unable to connect to the server. Please try again.');
                }
            });
        }
    },
    checkKeyPress: function (evt) {
        if (evt && evt.keyCode && evt.keyCode == 13)
            Ext.getCmp('login').doLogin();
    }
});

Ext.reg('login', NSMobile.views.Login);