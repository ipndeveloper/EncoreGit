NSMobile.views.Trip = Ext.extend(Ext.Panel, {
    iconCls: 'plane ' + syncClass,
    title: 'Trip',
    layout: 'card',
    width: '100%',
    initComponent: function () {
        NSMobile.stores.trip.proxy.url = plServiceURI + '/tripeurope/cy/' + getStorage('auth').country + '/lg/' + getStorage('auth').language + '/id/' + getStorage('auth').username;
        NSMobile.stores.trip.load();

        var tripPanel = new Ext.Panel({
            id: 'trippanel',
            scroll: 'vertical',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            tpl: [
                '<section class="KPIdrill pvList tripList">',
                    '<header class="x-list-header gridHeader PVheader"><span class="label term" data-term="PointsShownAs">Points are shown as of:</span> <span class="data">{[GetFormattedTripDate(values.LastMonthClosed)]}</span></header>',
                    '<section>',
                        '<div class="x-list-header performanceGroupHead term" data-term="YourCurrentActivity">Active Points</div>',
                        '<p><span class="label term" data-term="PersonalSalesPoints">Personal Sales</span> <span class="data">{PersonalSalesPointsActive}</span></p>',
                        '<p><span class="label term" data-term="ExcellenceSalesPoints">Excellence Sales</span> <span class="data">{ExcellenceSalesPointsActive}</span></p>',
                        '<p><span class="label term" data-term="QualifiedSponsors">Sponsors</span> <span class="data">{SponsoringPointsActive}</span></p>',
                        '<p><span class="label term" data-term="SponsorsInOct">Sponsors in Oct, Nov, Dec</span> <span class="data">{SponsorsForInfoActive}</span></p>',
                    '</section>',
                    '<section>',
                        '<div class="x-list-header performanceGroupHead term" data-term="PotentialActivityBalance">Potential Points</div>',
                        '<p><span class="label term" data-term="ExcellenceSalesPts">Excellence Sales</span> <span class="data">{ExcellenceSalesPointsPotential}</span></p>',
                        '<p><span class="label term" data-term="NonQualifiedSponsors">Sponsors</span> <span class="data">{SponsoringPointsPotential}</span></p>',
                    '</section>',
                    '<section>',
                        '<div class="x-list-header performanceGroupHead term" data-term="SalesProgress">Progress</div>',
                        '<p><span class="label term" data-term="Goal">Goal for Sales</span> <span class="data">{SalesPointsGoal}</span></p>',
                        '<p><span class="label term" data-term="Earned">Earned</span> <span class="data">{SalesPointsEarned}</span></p>',
                        '<p><span class="label term" data-term="Missing">Missing</span> <span class="data">{SalesPointsNeeded}</span></p>',
//                        '<p>',
//                            '<span class="label" id="tripmcp">Missing Counting Potential</span>',
//                            '<span class="data">You will earn trip if your potential points become active! {SalesPointsNeededAfterPotential}</span>',
//                        '</p>',
                    '</section>',
                '</section>'
            ]
        });

        var notEligiblePanel = new Ext.Panel({
            id: 'noteligiblepanel',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            html: [
                '<section class="standardMessage">',
                    '<span class="term" data-term="TripIneligible">Sorry,you won\'t go on the trip this year. But certainly next year.</span>',
                '</section>'
            ]
        });

        var noTripDataPanel = new Ext.Panel({
            id: 'notripdatapanel',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            html: [
                '<section class="standardMessage">',
                    '<span class="term" data-term="NotripData">No trip data is available at this time. Please check on the CBC for trip rules and promotional period dates.</span>',
                '</section>'
            ]
        });

        Ext.apply(this, {
            items: [
                tripPanel,
                notEligiblePanel,
                noTripDataPanel
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'European Excellence Club'
                }
            ]
        });

        NSMobile.views.Trip.superclass.initComponent.apply(this, arguments);
    }
});

function GetFormattedTripDate(dateString) {
    return new Date(dateString.substring(0, 4), parseInt(dateString.substring(4, 6)) - 1, 1).format("m/Y");
}

function fixTripTerms() {
    var termElms = document.querySelectorAll('.tripList .term,#noteligiblepanel .term,#notripdatapanel .term');
    for (var i = 0; i < termElms.length; i++) {
        var el = termElms[i];
        var termName = el.getAttribute('data-term');
        el.innerText = currentTerms[termName];
    }
}

Ext.reg('trip', NSMobile.views.Trip);