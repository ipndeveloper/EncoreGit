/**
 * Filestyle - jQuery Plugin
 * Style the file upload input with jQuery
 *
 * Examples and documentation at: https://github.com/microtroll/jquery-filestyle
 *
 * Copyright (c) 2014 microtroll
 *
 * Version: 1.7.1
 * Requires: jQuery v2+
 *
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 */

(function($) {

  $.fn.extend({

    filestyle: function(settings) {

      var o = {
        inputWidth: 150,
        inputHeight: 18,
        inputClass: '',
        buttonBgImage: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABoAAAAaCAYAAACpSkzOAAABc0lEQVR4Xr2V4W2DMBCFEyZgBEagygAkEzSdoM4ErTdoJ0g2gA3CBhUDRGED2KDeIH2Vvkr+EQxWrJ709HSRzcfdmXi9mojNZmNklVSi3+hRd7lcmlVErO8A9rKjVMzsHSUrYBsFAlLLDKmTGqmTHIBcqliTA2gEO8yCJiANb+sm2ppTtVkGA0S7zkAOS/vPHGvSl1AbM/yINzFDZq3EMwKR8VYFc7BSbFj2FlQ4WVHlVeME3sZQmGNDWoVAJUmHvzHsmOjwcgnI4XtpGwly86D78YynEaCeJPcqM2pfGQHI8X4JqMJb/Bwxq2oJqCMxPPiTqgppmDuF7DHeoQguHqSbVJOX0rd0Q1fpQ9rSUh9Us2YIzgi3XlVG30av5Ek60Y5SekU+xHjV2P/4U/VBrfaNSa+JwDe1oyvpLj7WHedgD13lrHsHFKgM0CMSqJBdpTwEA/SYOPJfIVgAlBQ2BkBpYQFQUtgpSw3ilO2k8Q+i3+wP/VCz/iKhnJkAAAAASUVORK5CYII=',
        buttonBgRepeat: 'no-repeat',
        buttonBgPosition: 'left center',
        buttonWidth: 50,
        buttonHeight: 18,
        buttonText: 'Browse'
      };
      settings = $.extend(o, settings);

      // wrapper div width
      var wrapWidth = o.inputWidth + o.buttonWidth + 10;

      // create wrapper
      $(this).wrap('<div class="file-wrap" />').css({
        'opacity': 0,
        'z-index': 9999,
        position: 'absolute',
        right: 0,
        top: 0,
        height: o.inputHeight,
        'padding': 0,
        'margin': 0
      });

      // create fake input and button
      $('.file-wrap').append('<input type="text" class="file-fake ' + o.inputClass + '" /><div class="button-fake">' + o.buttonText + '</div>').css({
        position: 'relative',
        height: o.inputHeight,
        width: wrapWidth
      });

      // fake button css
      $('.button-fake').css({
        position: 'absolute',
        top: 0,
        right: 0,
        'line-height': o.buttonHeight + 'px',
        'background-image': 'url(' + o.buttonBgImage + ')',
        'background-repeat': o.buttonBgRepeat,
        'background-position': o.buttonBgPosition,
        height: o.buttonHeight,
        width: o.buttonWidth,
        'cursor': 'pointer'
      });

      // fake input css
      $('.file-fake').css({
        position: 'absolute',
        top: 0,
        left: 0,
        'z-index': 1,
        width: o.inputWidth
      });

      // clone input value into the fake input
      $(this).map(function() {
        $(this).on('change', function() {
          var value = $(this).val();
          $(this).next('input').val(value);
        });
      });
    }

  });
})(jQuery);
