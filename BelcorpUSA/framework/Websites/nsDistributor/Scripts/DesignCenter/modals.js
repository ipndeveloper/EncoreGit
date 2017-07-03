

function hookModals() {
    //Login Modal (display only if they are not already logged in)
    if (!GetIsUserLoggedIn()) {
        $('#loggedOut').addClass('prompt');
        //        $('a.btnSignUp').hide();
        $('#btnLogin').after('<span><a href="javascript:void(0);" id="Skip" class="FL Button MinorButton btnSignUp jqmClose">Proceed without logging in</a></span>');
        $('#loginContainer').wrap('<div id="LoginPrompt" class="jqmWindow LModal"><div class="mContent modalContent"></div></div>').show();
        $('#ViewLibrary').hide();
    }
    else {
        $('#LoginPrompt').jqmHide();
        $('.jqmOverlay').jqmHide();
    }

    $('#btnClose').live('click', function () {
        $('#mediaLibrary').jqmHide();
    });

    $('#savingNovellasInProgress').jqm({
        modal: true,
        onShow: function (h) {
            h.w.css({
                top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + ($(window).scrollTop() - 20) + 'px',
                left: Math.floor(parseInt($(window).width() / 2)) + 'px',
                display: 'block'
            });
        },
        onHide: function (h) {
            h.w.fadeOut();
            if (h.o)
                h.o.fadeOut(function () { $(this).remove(); });
        }
    });

    //Modal
    $('#LoginPrompt').jqm({
        modal: true,
        onShow: function (h) {
            h.o.fadeIn();
            h.w.css({
                top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) - 110 + 'px',
                left: Math.floor($('div.TopInner').width() / 2) - Math.floor(h.w.width() / 2) + 'px'
            }).fadeIn();
        },
        onHide: function (h) {
            h.w.fadeOut();
            if (h.o)
                h.o.fadeOut(function () { $(this).remove(); });
            $('#Skip').remove();
            $('a.btnSignUp').show();
            $('#loggedOut').removeClass('prompt');
            $('#loginContainer').unwrap().unwrap().hide();
        }
    }).jqmShow();


    //Preview Design Modal
    $('#PreviewDesign').jqm({
        modal: true,
        onShow: function (h) {
            h.o.fadeIn();
            h.w.css({
                top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + ($(window).scrollTop() - 20) + 'px',
                left: Math.floor(parseInt($(window).width() / 2)) - Math.floor(h.w.width() / 2) + 'px'
            }).fadeIn();

            $('div.item').disableSelection();
            $('div.designItems').sortable({
                update: function () {
                    var status = $("div.designItems").attr("id");
                    var rowArray = $("div.designItems").sortable('toArray');
                    $.ajax({
                        type: "Get",
                        url: '/Design/ReorderIndex',
                        data: { rows: rowArray },
                        success: function (data) {
                            //success case update
                        },
                        dataType: "json",
                        traditional: true
                    });
                }
            });

            //Shrink images to fit in window until up to a set percentage, then scroll to 
            //view the rest (prevents from making images too small if there are a lot of images
            var divWidth = $('div.designItems').width();
            var totalWidth = 0,
            countItems = 0;
            var counter = $('#designPreviewContainer img').length;
            $('div.designItems img').each(function () {
                $(this).load(function () {
                    totalWidth += this.width;
                    countItems++;
                    if (countItems == counter) {
                        var percent = divWidth / totalWidth;
                        if (totalWidth > divWidth) {
                            if (percent < .4) {
                                percent = .4;
                                $("div.designItems").width(totalWidth * percent);
                            }
                            $('#PreviewDesign').css('top', Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt((h.w.height() * percent) / 2)) + ($(window).scrollTop() - 20) + 'px');
                            ShrinkPreviewImageSize(percent); //Added this function to functions.js line 209
                        }
                        else {
                            var itemSpacing = Math.floor((divWidth - totalWidth) / (2 * countItems));
                            $('div.designItems .item').css({ 'margin-left': itemSpacing, 'margin-right': itemSpacing });
                        }
                    }
                });
            });
        },
        onHide: function (h) {
            h.w.fadeOut();
            if (h.o)
                h.o.fadeOut(function () { $(this).remove(); });
        }
    });

    //ChangesLost Modal
    $('#ChangesLost').jqm({
        modal: false,
        onShow: function (h) {
            h.o.fadeIn();
            h.w.css({
                top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + ($(window).scrollTop() - 20) + 'px',
                left: Math.floor(parseInt($(window).width() / 2)) + 'px'
            }).fadeIn();
        },
        onHide: function (h) {
            h.w.fadeOut();
            if (h.o)
                h.o.fadeOut(function () { $(this).remove(); });
        }
    });
    $('#ChangesLost').jqmAddClose('#changesLostLink');

    //Library Modal
    $('#mediaLibrary').jqm({
        modal: true,
        onShow: function (h) {
            $('#mediaLibrary').fadeIn('fast').trigger('resizeHeaders');
            h.w.fadeIn();
        },
        onHide: function (h) {
            h.w.fadeOut();
            refreshRecent();
            if (h.o)
                h.o.fadeOut(function () { $(this).remove(); });
        }
    });

    //Edit_Approved_Novella Modal
    $('#Edit_Approved_Novella').jqm({
        modal: true,
        onShow: function (h) {
            h.o.fadeIn();
            h.w.css({
                top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + ($(window).scrollTop() - 20) + 'px',
                left: Math.floor(parseInt($(window).width() / 2)) + 'px'
            }).fadeIn();
        },
        onHide: function (h) {
            h.w.fadeOut();
            if (h.o)
                h.o.fadeOut(function () { $(this).remove(); });
        }
    });


    //Enlarge the base item thumbnail
    $('div.baseThumb').toggle(function () {
        if ($(this).parent().height() < 240) {
            var imgHeight = $(this).parent.height() - 10;
        } else {
            imgHeight = 230;
        }
        $('div.baseThumb img').animate({
            height: imgHeight + 'px'
        }, 500, function () {
            // Animation Complete
        });
    }, function () {
        $('div.baseThumb img').animate({
            height: '40px'
        }, 500, function () {
            // Animation Complete
        });
    });

    $('#ViewLibrary,#OpenImage').live('click', function (e) {
        if ($(e.target).data('oneclicked') != 'yes') {
            getUserGalleryLibrary();
        }
        $(e.target).data('oneclicked', 'yes');

        $('#mediaLibrary').jqmShow();
    });
}

function GetIsUserLoggedIn() {
    var result = false;
    $.ajax({
        type: 'GET',
        dataType: "json",
        url: '/Design/IsUserLoggedIn',
        async: false,
        complete: function(data) {
            result = data.responseText != null && data.responseText.toLowerCase() == 'true';
        }
    });
    return result;
}

           
