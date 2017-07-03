$(function () {
    if (!('placeholder' in document.createElement('input'))) {
        $.watermark.options.className = 'placeholder';
        $.watermark.options.hideBeforeUnload = false;
        $('[placeholder]').each(function (num, e) {
            $(e).watermark($(e).attr('placeholder'));
        })
    }
});