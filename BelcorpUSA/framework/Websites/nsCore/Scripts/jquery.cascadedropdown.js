(function ($)
{
	/*example
	$('#parentID').cascadedropdown({
	url: '/path/to/action',
	paramName: 'actionParamName',
	childSelect: $('#childID')
	});
    
	*/
	$.fn.cascadedropdown = function (options)
	{
		var defaults = { addBlankFirst: true, blankText:'' };
		var opts = $.extend(defaults, options);

		return this.each(function ()
		{
			$(this).change(function ()
			{
				var selectedValue = $(this).val();
				var params = {};
				params[opts.paramName] = selectedValue;
				$.getJSON(opts.url, params, function (items)
				{
					opts.childSelect.empty();
					if (opts.addBlankFirst)
						items.unshift({ Id: -1, Name: opts.blankText });
					$.each(items, function (index, item)
					{
						opts.childSelect.append(
                            $('<option/>')
                                .attr('value', item.Id)
                                .text(item.Name)
                        );
					});
				});
			});
		});
	};
})(jQuery);