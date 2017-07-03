(function () {
    window.commissionRunner = {};

    commissionRunner.App = {
        prepCommissionRun: false,
        publishCommission: false,
        commissionRunTotalSteps: 0,
        commissionRunCurrentStep: 0,
        commissionRunNextStep: 0,
        commissionRunProgress: '0%',
        systemEventApplicationID: 0,
        setupClickHandlers: function () {
            $('#btnPublish').click(function () {
                if (commissionRunner.App.periodConfirmed()) {
                    commissionRunner.App.disablePage();
                    $.ajax({
                        type: 'POST',
                        cache: false,
                        async: false,
                        url: '/Commissions/Publish/ExecuteCommissionPublish',
                        dataType: 'json',
                        data: { planID: $('#SelectedPlanID').val(), periodID: $('#SelectedPeriodID').val() },
                        success: function (json) {
                            commissionRunner.App.updateCommissionRunnerApp(json);
                            if (json.result) {
                                commissionRunner.App.publishCommission = true;
                                commissionRunner.App.executeCommissionRunStep();
                            } else {
                                commissionRunner.App.enablePage();
                                commissionRunner.App.publishCommission = false;
                            }
                        },
                        error: function (json) {
                            commissionRunner.App.enablePage();
                            commissionRunner.App.publishCommission = false;
                            alert('Commission Publish Error Has Occurred.');
                        }
                    });
                }
            });

            $('#btnClose').click(function () {
                if (commissionRunner.App.closeCommissionRunConfirmed()) {
                    commissionRunner.App.disablePage();
                    $.ajax({
                        type: 'POST',
                        cache: false,
                        async: false,
                        url: '/Commissions/Runner/CloseSystemEvents',
                        dataType: 'json',
                        data: { systemEventApplicationID: commissionRunner.App.systemEventApplicationID },
                        success: function (json) {
                            commissionRunner.App.enablePage();
                            if (json.result) {
                                commissionRunner.App.enablePageControls();
                                $('#RunProgress').empty();
                            } else {
                                alert('Failed to close open commission run.');
                            }
                        },
                        error: function (json) {
                            commissionRunner.App.enablePage();
                            alert('Failed to close open commission run.');
                        }
                    });
                }
            });

            $('#btnRun').click(function () {
                if (commissionRunner.App.periodConfirmed()) {
                    commissionRunner.App.disablePage();
                    $.ajax({
                        type: 'POST',
                        cache: false,
                        async: false,
                        url: '/Commissions/Runner/ExecuteCommissionRun',
                        dataType: 'json',
                        data: { planID: $('#SelectedPlanID').val(), periodID: $('#SelectedPeriodID').val() },
                        success: function (json) {
                            commissionRunner.App.updateCommissionRunnerApp(json);
                            if (json.result) {
                                commissionRunner.App.prepCommissionRun = true;
                                commissionRunner.App.executeCommissionRunStep();
                            } else {
                                commissionRunner.App.enablePage();
                                commissionRunner.App.prepCommissionRun = false;
                            }
                        },
                        error: function (json) {
                            commissionRunner.App.enablePage();
                            commissionRunner.App.prepCommissionRun = false;
                            alert('Commission Run Error Has Occurred.');
                        }
                    });
                }
            });

            $('#SelectedPlanID').change(function () {
                $.ajax({
                    type: 'POST',
                    cache: false,
                    async: false,
                    url: '/Commissions/Runner/GetPeriodForPlan',
                    dataType: 'json',
                    data: { planID: $('#SelectedPlanID').val() },
                    success: function (json) {
                        var periods = json.periods;
                        if (json.result) {
                            commissionRunner.App.updateSelectedPlan(periods);
                        }
                    },
                    error: function (json) {
                        alert('Commission Run Error Has Occurred.');
                    }
                });
            });
        },

        executeCommissionRunStep: function () {
            $.ajax({
                type: 'POST',
                cache: false,
                async: false,
                url: commissionRunner.App.prepCommissionRun == true ? '/Commissions/Runner/ExecuteCommissionRunStep' : '/Commissions/Publish/ExecuteCommissionPublishStep',
                dataType: 'json',
                success: function (json) {
                    if (json.result) {
                        commissionRunner.App.updateCommissionRunStep(json);
                    } else {
                        commissionRunner.App.finishCommissionRun(json);
                    }
                },
                error: function (json) {
                    commissionRunner.App.finishCommissionRun(json);
                }
            });
        },

        init: function () {
            commissionRunner.App.setupClickHandlers();
            commissionRunner.App.disableRunStatus();
            commissionRunner.App.disableInProgressImage();
        },

        closeCommissionRunConfirmed: function () {
            var closeRun = window.confirm("Are you sure you want to close the open commission run?");
            return closeRun;
        },

        periodConfirmed: function () {
            var period = window.prompt("Please confirm the Period you have selected by typing it in as displayed.");
            if (period != null && $('#SelectedPeriodID').val() == period.replace('-', '')) {
                return true;
            }
            else {
                alert('The Period Selected in the drop down and Period Entered in the previous text box do not match!');
                return false;
            }
        },

        disablePage: function () {
            $("#RunSettings").hide();
            $("#RunStatusColumn").css('width', '99%');
            commissionRunner.App.clearProgress();
            commissionRunner.App.enableInProgressImage();
            commissionRunner.App.enableRunStatus();
        },

        enablePage: function () {
            $("#RunSettings").show();
            $("#RunStatusColumn").css('width', '60%');
            commissionRunner.App.disableInProgressImage();
        },

        disablePageControls: function () {
            $('#btnRun').hide();
            $('#btnPublish').hide();
            $('#btnClose').show();
            $('#SelectedPlanID').attr('disabled', true);
            $('#SelectedPeriodID').attr('disabled', true);
        },

        enablePageControls: function () {
            $('#btnRun').show();
            $('#btnPublish').show();
            $('#btnClose').hide();
            $('#SelectedPlanID').removeAttr('disabled');
            $('#SelectedPeriodID').removeAttr('disabled');
        },

        disableInProgressImage: function () {
            $('#InProgress').hide();
        },

        enableInProgressImage: function () {
            $('#InProgress').show();
        },

        disableRunStatus: function () {
            $('#RunStatusHeading').hide();
        },

        enableRunStatus: function () {
            $('#RunStatusHeading').show();
        },

        clearProgress: function () {
            $('#RunProgress, #PercentCompleteMessage').empty();
        },

        setCommissionRunInProgressMessage: function (message) {
            var html = '<li>' + message + '</li>';
            $(html).prependTo('#RunProgress').slideDown("slow");
        },

        generateEventLogListItem: function (eventLog) {
            var html = '<li>' + eventLog.eventMessage + ' - ' + eventLog.createdDate + '</li>';
            $(html).prependTo('#RunProgress').slideDown("slow");
            $('#PercentCompleteMessage').html('Progress ' + commissionRunner.App.commissionRunProgress);
        },

        updateSelectedPlan: function (periods) {
            $('#SelectedPeriodID >option').remove();
            $.each(periods, function (i) {
                $("<option>").val(this.PeriodID)
										     .text(this.Value)
										     .appendTo('#SelectedPeriodID');
            });
        },

        updateCommissionRunnerApp: function (json) {
            commissionRunner.App.systemEventID = json.systemEventID;
            commissionRunner.App.commissionRunTotalSteps = json.totalSteps;
            commissionRunner.App.commissionRunCurrentStep = json.currentStep;
            commissionRunner.App.commissionRunNextStep = json.nextStep;
            commissionRunner.App.commissionRunProgress = json.currentCommissionRunProgress;
            commissionRunner.App.generateEventLogListItem(json.commissionRunSystemEventLog);
            if (json.displaycommissionRunStepSystemEventLog) {
                commissionRunner.App.generateEventLogListItem(json.commissionRunStepSystemEventLog);
            }
        },

        updateCommissionRunStep: function (json) {
            commissionRunner.App.commissionRunCurrentStep = json.currentStep;
            commissionRunner.App.commissionRunProgress = json.currentCommissionRunProgress;
            commissionRunner.App.generateEventLogListItem(json.commissionRunStepSystemEventLog);
            commissionRunner.App.updateSelectedPlan(json.periods);
            commissionRunner.App.executeCommissionRunStep()
        },

        finishCommissionRun: function (json) {
            commissionRunner.App.commissionRunProgress = json.currentCommissionRunProgress;
            commissionRunner.App.generateEventLogListItem(json.commissionRunSystemEventLog);
            commissionRunner.App.updateSelectedPlan(json.periods);
            commissionRunner.App.enablePage();
            commissionRunner.App.prepCommissionRun = false;
            commissionRunner.App.publishCommission = false;
        }

    }

})();