NSMobile.views.Parties = Ext.extend(Ext.Panel, {
    title: 'Parties',
    iconCls: 'team',
    cls: 'Orders',
    layout: 'card',
    fullscreen: true,
    cardSwitchAnimation: false,
    currentParty: 0,
    previousCard: null,
    initComponent: function () {
        NSMobile.stores.partyorders.load();
        loadAllContacts();
        var canUsePhone = navigator.service !== undefined;
        if (canUsePhone)
            loadPhoneContacts();

        var partylist = new Ext.Panel({
            items: [
                {
                    fullscreen: isDesktop,
                    height: baseListHeight,
                    xtype: 'list',
                    fullscreen: true,
                    store: NSMobile.stores.partyorders,
                    itemTpl: [
                        '<article class="newsArticle articleType1">',
				            '<div class="articleBody">',
				                '<span class="newsTitle newsTitle1">{name}</span>',
				                '<span class="newsSummary">{date}</span>',
				            '</div>',
                        '</article>'
                    ],
                    listeners: {
                        itemtap: {
                            fn: function (object, index, item, e) {
                                var basedata = object.store.getAt(index).data;
                                var data = basedata.partydetail;
                                Ext.getCmp('partydetails').update(data);
                                Ext.getCmp('guestlist').update(data.guests);
                                Ext.getCmp('parties').currentParty = index;
                                Ext.getCmp('parties').updateAndSetActiveItem('partyinfo', 1);
                            }
                        }
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'My Parties'
                }
            ]
        });

        var infotpl = [
            '<section class="detail articleView partyHeader">',
				'<arcticle>',
					'<header>',
						'<h1 class="articleTitle">{name}</h1>',
						'<div class="left">',
                            '<span>{date}</span>',
                            '<span>{time}</span>',
                        '</div>',
                        '<div class="right">',
                            '<span>{address1} {address2}</span> <span>{city}, {state} {postalcode}</span></span>',
                        '</div>',
					'</header>',
				'</article>',
            '</section>'
        ];

        var partydetails = new Ext.Panel({
            id: 'partydetails',
            tpl: [
                '<section class="detailsList partyActions">',
                    '<div class="detailRow clickableRow" onclick="javascript:(function(){Ext.getCmp(\'parties\').viewGuestList()})()">My Guest List <span class="clickIcon"></span></div>',
                    '<div class="detailRow clickableRow" onclick="javascript:(function(){Ext.getCmp(\'parties\').viewAddGuest()})()">Add a Guest <span class="clickIcon"></span></div>',
					'<div class="detailRow clickableRow" onclick="javascript:(function(){Ext.getCmp(\'parties\').viewInviteGuest()})()">Send Party Invitations <span class="clickIcon"></span></div>',
                '</section>',
                '<section class="partyStats">',
                    '<p><span class="label">Number of people invited</span><span class="data">{inviteCount}</span></p>',
                    '<p><span class="label">Guests attending</span><span class="data">{guestsAttending}</span></p>',
                    '<p><span class="label">Can\'t make it</span><span class="data">{guestsNotAttending}</span></p>',
                    '<p><span class="label">Online purchases</span><span class="data">${onlinePurchases}</span></p>',
                '</section>'
            ]
        });

        var partylanding = new Ext.Panel({
            fullscreen: true,
            scroll: 'vertical',
            cls: 'Orders',
            items: [
                {
                    id: 'partyinfo',
                    tpl: infotpl
                },
                partydetails
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Party Overview',
                    items: [
                        {
                            id: 'partybackbutton',
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('parties').layout.prev(pageBackAnimation);
                            }
                        }
                    ]
                }
            ]
        });

        var guestlist = new Ext.Panel({
            fullscreen: true,
            items: [
                {
                    id: 'guestpartyinfo',
                    tpl: infotpl
                },
                {
                    xtype: 'list',
                    id: 'guestlist',
                    fullscreen: true,
                    store: NSMobile.stores.partyGuests,
                    disableSelection: true,
                    itemTpl: [
                         '<section class="guestList">',
							'<div class="guestPanel <tpl if="rsvp === false">rsvpno</tpl>">',
								'<div class="left">',
									'<span class="name">{firstName} {lastName}</span>',
									'<span class="info">',
										'<tpl if="rsvp === true"><span class="rsvpyes">RSVP: Yes</span></tpl>',
										'<tpl if="rsvp === false"><span class="rsvpno">RSVP: No</span></tpl>',
										'<tpl if="rsvp === null"><span class="rsvp">RSVP: Pending</span></tpl>',
										'<tpl if="onlinetotal.length &gt; 0"><span class="onlinetotal">, ${onlinetotal}</span></tpl>',
									'</span>',
								'</div>',
								'<div class="right"><div class="icon-panel <tpl if="customer === true">icon-active</tpl>" onclick="javascript:(function(){Ext.getCmp(\'parties\').toggleGuestCart({[xindex - 1]})})()"><span class="icon-guestCart"></span></div></div>',
							'</div>',
						'</section>'
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Guest List',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('parties').layout.prev(pageBackAnimation);
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            html: '<div class="icon-container"></div>',
                            cls: 'icon-addButton',
                            handler: function () {
                                Ext.getCmp('parties').viewAddGuest();
                            }
                        }
                    ]
                }
            ]
        });

        this.addguestlocationpopup = new Ext.Panel({
            floating: true,
            modal: true,
            cls: 'responsePop',
            centered: true,
            height: 200,
            width: 300,
            items: [
                {
                    html: 'Which contact list would you like to add a party guest from?'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Select Contact List'
                },
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        {
                            disabled: !canUsePhone,
                            text: 'My Phone',
                            handler: function () {
                                Ext.getCmp('parties').addguestlocationpopup.hide();
                                Ext.getCmp('addguestlist').bindStore(NSMobile.stores.phoneContacts);
                                Ext.getCmp('parties').addguestlist.show('pop');
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            text: 'Workstation',
                            handler: function () {
                                Ext.getCmp('parties').addguestlocationpopup.hide();
                                Ext.getCmp('addguestlist').bindStore(NSMobile.stores.allContacts);
                                Ext.getCmp('parties').addguestlist.show('pop');
                            }
                        }
                    ]
                }
            ]
        });

        this.addguestsuccesspopup = new Ext.Panel({
            floating: true,
            modal: true,
            centered: true,
            height: 200,
            width: 300,
            cls: 'responsePop',
            items: [
                {
                    tpl: '{name} was successfully added to the party'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Guest Added'
                },
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        {
                            text: 'Guest List',
                            handler: function () {
                                document.getElementById('guestaddform').reset();
                                Ext.getCmp('parties').addguestsuccesspopup.hide();
                                Ext.getCmp('parties').setActiveItem(2, pageBackAnimation);
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            text: 'Add another',
                            handler: function () {
                                document.getElementById('guestaddform').reset();
                                Ext.getCmp('parties').addguestsuccesspopup.hide();
                            }
                        }
                    ]
                }
            ]
        });

        this.addguestlist = new Ext.Panel({
            floating: true,
            modal: true,
            centered: true,
            height: 400,
            width: 300,
            //cls: 'responsePop',
            items: [
                {
                    xtype: 'list',
                    grouped: true,
                    width: 288, //parent - 12
                    height: 341, //parent - 59
                    store: NSMobile.stores.allContacts,
                    id: 'addguestlist',
                    itemTpl: [
                        '<article class="newsArticle articleType1">',
				            '<div class="articleBody">',
				                '<span class="newsTitle newsTitle1">{firstName} {lastName}</span>',
				                '<span class="newsSummary">{email}</span>',
				            '</div>',
                        '</article>'
                    ],
                    listeners: {
                        itemTap: {
                            fn: function (object, index, item, e) {
                                var data = object.store.getAt(index).data;
                                document.getElementById('addguestfname').value = data.firstName;
                                document.getElementById('addguestlname').value = data.lastName;
                                document.getElementById('addguestemail').value = data.email;
                                Ext.getCmp('parties').addguestlist.hide();
                            }
                        }
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Select Contact'
                }
            ]
        });

        var addguest = new Ext.Panel({
            fullscreen: true,
            cls: 'Orders',
            items: [
                {
                    id: 'addguestpartyinfo',
                    tpl: infotpl
                },
                {
                    html: [
                        '<form id="guestaddform" onsubmit="return Ext.getCmp(\'parties\').submitAddGuest()">',
                            '<section class="partyStats inputForm">',
                                '<div class="inputRow">',
                                    '<input type="text" id="addguestfname" required="required" placeholder="First name" />',
									'<div class="rightIcon"><a href="#" class="icon-openList" onclick="javascript:(function(){Ext.getCmp(\'parties\').addGuestExisting()})()"></a></div>',
                                '</div>',
                                '<div class="inputRow">',
                                    '<input type="text" id="addguestlname" required="required" placeholder="Last name" />',
                                '</div>',
                                '<div class="inputRow">',
                                    '<input type="email" id="addguestemail" required="required" placeholder="Email" />',
                                '</div>',
                            '</section>',
						    '<section class="formSubmit">',
						        '<input type="submit" onclick="javascript:(function(){Ext.getCmp(\'parties\').blurFields()})()" value="Add Guest" />',
						    '</section>',
						'</form>'
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Add Guest',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('parties').setActiveItem(Ext.getCmp('parties').previousCard, pageBackAnimation);
                            }
                        }
                    ]
                }
            ]
        });

        this.inviteguestlocationpopup = new Ext.Panel({
            floating: true,
            modal: true,
            centered: true,
            height: 200,
            width: 300,
            cls: 'responsePop',
            items: [
                {
                    html: 'Which contact list would you like to invite a party guest from?'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Select Contact List'
                },
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        {
                            disabled: !canUsePhone,
                            text: 'My Phone',
                            handler: function () {
                                Ext.getCmp('parties').inviteguestlocationpopup.hide();
                                Ext.getCmp('inviteguestlist').bindStore(NSMobile.stores.phoneContacts);
                                Ext.getCmp('parties').inviteguestlist.show('pop');
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            text: 'Workstation',
                            handler: function () {
                                Ext.getCmp('parties').inviteguestlocationpopup.hide();
                                Ext.getCmp('inviteguestlist').bindStore(NSMobile.stores.allContacts);
                                Ext.getCmp('parties').inviteguestlist.show('pop');
                            }
                        }
                    ]
                }
            ]
        });

        this.inviteguestsuccesspopup = new Ext.Panel({
            floating: true,
            modal: true,
            centered: true,
            height: 200,
            width: 300,
            cls: 'responsePop',
            items: [
                {
                    tpl: 'You have sent an invitation to {name}.'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Invitation sent'
                },
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        {
                            text: 'Party stats',
                            handler: function () {
                                document.getElementById('guestinviteform').reset();
                                Ext.getCmp('parties').inviteguestsuccesspopup.hide();
                                Ext.getCmp('parties').setActiveItem(1, pageBackAnimation);
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            text: 'Invite more',
                            handler: function () {
                                document.getElementById('guestinviteform').reset();
                                Ext.getCmp('parties').inviteguestsuccesspopup.hide();
                            }
                        }
                    ]
                }
            ]
        });

        this.inviteguestlist = new Ext.Panel({
            floating: true,
            modal: true,
            centered: true,
            height: 400,
            width: 300,
            //cls: 'responsePop',
            items: [
                {
                    xtype: 'list',
                    grouped: true,
                    width: 288, //parent - 12
                    height: 341, //parent - 59
                    store: NSMobile.stores.allContacts,
                    id: 'inviteguestlist',
                    itemTpl: [
                        '<article class="newsArticle articleType1">',
				            '<div class="articleBody">',
				                '<span class="newsTitle newsTitle1">{firstName} {lastName}</span>',
				                '<span class="newsSummary">{email}</span>',
				            '</div>',
                        '</article>'
                    ],
                    listeners: {
                        itemTap: {
                            fn: function (object, index, item, e) {
                                var data = object.store.getAt(index).data;
                                document.getElementById('inviteguestname').value = data.firstName + ' ' + data.lastName;
                                document.getElementById('inviteguestemail').value = data.email;
                                Ext.getCmp('parties').inviteguestlist.hide();
                            }
                        }
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Select Contact'
                }
            ]
        });

        var inviteguest = new Ext.Panel({
            fullscreen: true,
            cls: 'Orders',
            items: [
                {
                    id: 'inviteguestpartyinfo',
                    tpl: infotpl
                },
                {
                    html: [
                        '<form id="guestinviteform" onsubmit="return Ext.getCmp(\'parties\').submitInviteGuest()">',
                            '<section class="partyStats inputForm">',
                                '<div class="inputRow">',
                                    '<input type="text" id="inviteguestname" required="required" placeholder="Name" />',
                                    '<div class="rightIcon"><a href="#" class="icon-openList" onclick="javascript:(function(){Ext.getCmp(\'parties\').inviteGuestExisting()})()" /></a></div>',
                                '</div>',
                                '<div class="inputRow">',
                                    '<input type="text" id="inviteguestemail" required="required" placeholder="Email" />',
                                '</div>',
                                '<div class="inputRow">',
                                    '<input type="text" id="inviteguestmessage" required="required" placeholder="Custom message" />',
                                '</div>',
                            '</section>',
						    '<section class="formSubmit">',
						        '<input type="submit" onclick="javascript:(function(){Ext.getCmp(\'parties\').blurFields()})()" value="Send Invitation" />',
						    '</section>',
						'</form>'
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Invite Guest',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('parties').setActiveItem(1, pageBackAnimation);
                            }
                        }
                    ]
                }
            ]
        });

        Ext.apply(this, {
            items: [
                partylist,
                partylanding,
                guestlist,
                addguest,
                inviteguest
            ]
        });

        NSMobile.views.Parties.superclass.initComponent.apply(this, arguments);
    },

    updateAndSetActiveItem: function (infoPanel, itemIndex) {
        var partyindex = this.currentParty;
        var partydata = NSMobile.stores.partyorders.getAt(partyindex).data;
        var detaildata = partydata.partydetail;
        detaildata.name = partydata.name;
        Ext.getCmp(infoPanel).update(detaildata);
        this.setActiveItem(itemIndex);
    },

    viewGuestList: function () {
        this.updateAndSetActiveItem('guestpartyinfo', 2);
    },

    viewAddGuest: function () {
        this.previousCard = this.items.indexOf(this.layout.getActiveItem());
        this.updateAndSetActiveItem('addguestpartyinfo', 3);
    },

    viewInviteGuest: function () {
        this.updateAndSetActiveItem('inviteguestpartyinfo', 4);
    },

    addGuestExisting: function () {
        this.addguestlocationpopup.show('pop');
    },

    inviteGuestExisting: function () {
        this.inviteguestlocationpopup.show('pop');
    },

    submitAddGuest: function () {
        var firstname = document.getElementById('addguestfname').value;
        var lastname = document.getElementById('addguestlname').value;
        var email = document.getElementById('addguestemail').value;
        if (firstname && lastname && email) {
            //todo: save/send to server, update guest list
            this.addguestsuccesspopup.show('pop');
            this.addguestsuccesspopup.items.first().update({ name: firstname + ' ' + lastname });
        }

        return false;
    },

    submitInviteGuest: function () {
        var name = document.getElementById('inviteguestname').value;
        var email = document.getElementById('inviteguestemail').value;
        var message = document.getElementById('inviteguestmessage').value;
        if (name && email && message) {
            //todo: save/send to server, update guest list
            this.inviteguestsuccesspopup.show('pop');
            this.inviteguestsuccesspopup.items.first().update({ name: name });
        }

        return false;
    },

    toggleGuestCart: function (index) {
        var row = document.querySelectorAll('section.guestList')[index]; //LOOK AT THIS HOTNESS
        var iconEl = row.querySelector('div.icon-panel');
        if (iconEl.classList)
            iconEl.classList.toggle('icon-active');
        else {
            if (iconEl.className.indexOf('icon-active') > -1)
                iconEl.className = iconEl.className.replace('icon-active', '').trim();
            else
                iconEl.className += ' icon-active';
        }
        //todo: server stuff
    },

    blurFields: function () {
        var fields = document.querySelectorAll('#addguestfname,#addguestlname,#addguestemail,#inviteguestname,#inviteguestemail,#inviteguestmessage');
        for (var i = 0; i < fields.length; i++)
            fields[i].blur();
    }
});

Ext.reg('parties', NSMobile.views.Parties);
