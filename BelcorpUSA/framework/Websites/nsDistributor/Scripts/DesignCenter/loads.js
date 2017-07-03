
function doCanvasStuff() {
    window.onload = function () {
        var elems = $('#novellaDesignItems_UL li img:first-child');
        var count = elems.length;

        elems.each(function (i) {
            var item = this,
            $item = $(item),
            width = item.naturalWidth || $item.outerWidth(),
            height = item.naturalHeight || $item.outerHeight(),
            frame = { width: width, height: height },
            border = $('div.designPreview .brdr'),
            borderWidth = border.width();

            frame.difference = (borderWidth - frame.width) / 2;
            $item.parent().data('frame', frame);

            if (i + 1 == count) {
                onLoadForDesignCenter();
                onDesignInit();
                onLoggedIn();
                hookModals();
            }
        });
    };

}


function onLoadForDesignCenter() {

    $("#ShareThis").click(function () {
        var bool = $(this).is(':checked');
        $('#novellaDesignItems_UL li.selected').find('input:hidden[name="IsSharedVal"]').val(bool);
        SaveIsShareable(bool);
    });
    var startOverResult;
    if ($('#ProductID').val() !== "" || $('#OrderItemID').val() !== "") {
        $.get('/Shop/GetStartOverLink', { ProductId: 11, bundleGuid: "", isForBundled: false }, function (result) {
            startOverResult = result;
        });
    } else {
        $.get('/Shop/GetStartOverLink', { ProductId: $('#BundleProductId').val(), bundleGuid: $('#BundleID').val(), isForBundled: true },
            function (result) {
                startOverResult = result;
            });
    }

    $('#StartFromScratch').click(function (event) {
        if (startOverResult.success) {
            window.location.href = startOverResult.message;
            event.stopPropagation();
        } else {
            showMessage(startOverResult.message, true);
        }
    });
}

function allNovellasHasPreviews() {
    var $lis = $('#novellaDesignItems_UL li');

    var notediting = $lis.find('input:hidden[name="IsEditing"]').filter('[value="False"]').length;
    return notediting == $lis.length;
}

function declareCallBackInitalizer(isNew, numRequest) {
    if (isNew) {
        mainButtonsUI(false);
        $.ajaxDoneCallBackInitalizerG.rc = new MyRequestsCompleted({
            numRequest: numRequest,
            singleCallBack: function () {
                $('#savingNovellasInProgress').jqmHide();
                selectNextUnapprove();
            }
        });
    }
}




