NSMobile.models.PartyDetail = Ext.regModel('PartyDetail', {
    fields: [
        'address1',
        'address2',
        'postalCode',
        'city',
        'state',
        'date',
        'time',
        'inviteCount',
        'guestsAttending',
        'guestsNotAttending',
        'onlinePurchases',
        { name: 'guests', type: 'Contact' }
    ]
});