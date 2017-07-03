/*
* Copyright (c) 2010 Daniel Stafford
* This is licensed under GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*/

(function ($) {

	$.fn.jsonSuggest = function (searchData, settings) {
		var defaults = {
			minCharacters: 1,
			maxResults: undefined,
			wildCard: "",
			caseSensitive: false,
			notCharacter: "!",
			maxHeight: 350,
			highlightMatches: true,
			onSelect: undefined,
			ajaxResults: false,
			width: undefined,
			source: undefined,
			imageSize: undefined,
			delay: 400,
			data: {},
			showMore: false,
			canCollapseGroup: true,
			startCollapsed: true,
			noResults: 'There are no results for the specified criteria.',
			loading: 'loading...',
			more: 'more',
			defaultToFirst: true
		};
		settings = $.extend(defaults, settings);
		if (!settings.source)
			settings.source = $(this);

		function scrollbarWidth() {
			var div = $('<div style="width:50px;height:50px;overflow:hidden;position:absolute;top:-200px;left:-200px;" id="jsonSuggScrollBar><div style="height:100px;"></div></div>');
			// Append our div, do our calculation and then remove it
			$('body').append(div);
			var w1 = $('div', div).innerWidth();
			div.css('overflow-y', 'scroll');
			var w2 = $('div', div).innerWidth();
			$(div).remove();
			return (w1 - w2);
		}

		var originalSearchData = searchData, func = 'function', string = 'string', number = 'number', sbWidth = scrollbarWidth();

		arguments.callee.selectItem = function (item) {
			if (settings.source) {
				$(settings.source).val(item.text).parent().find('.jsonSuggestResults').empty().hide();
				if (typeof (settings.onSelect) === func) {
					settings.onSelect.apply($(settings.source), [item]);
				}
			}
		};

		return this.each(function () {

			function regexEscape(txt, omit) {
				var specials = ['/', '.', '*', '+', '?', '|',
								'(', ')', '[', ']', '{', '}', '\\'], escapePatt;

				if (omit) {
					for (var i = 0; i < specials.length; i++) {
						if (specials[i] === omit) { specials.splice(i, 1); }
					}
				}

				escapePatt = new RegExp('(\\' + specials.join('|\\') + ')', 'g');
				return txt.replace(escapePatt, '\\$1');
			}

			var obj = $(this),
				wildCardPatt = new RegExp(regexEscape(settings.wildCard || ''), 'g'),
				results = $('<div class="jsonSuggestResults"></div>'),
				currentSelection, pageX, pageY, match, searchXHR, waitTimeout, searchingFor;

			// When an item has been selected then update the input box,
			// hide the results again and if set, call the onSelect function
			function selectResultItem(item) {
				obj.val(item.text);
				results.empty().hide();

				if (typeof (settings.onSelect) === func) {
					var parent = $(currentSelection).parent().parent();
					settings.onSelect.apply(obj, [item, currentSelection && parent.is('.resultGroup') ? parent.attr('id').replace(/^resultGroup/, '') : undefined]);
				}
			}

			// Used to get rid of the hover class on all result item elements in the
			// current set of results and add it only to the given element. We also
			// need to set the current selection to the given element here.
			function setHoverClass(el) {
				$('div.resultItem.hover', results).removeClass('hover');
				$(el).addClass('hover');

				currentSelection = el;
			}

			// Build the results HTML based on an array of objects that matched
			// the search criteria, highlight the matches if feature is turned on in
			// the settings.
			function buildResults(resultObjects, sFilterTxt) {
				var oddRow = true, max, hasMore,
					filterPatt = new RegExp('(' + sFilterTxt + ')', settings.caseSensitive ? 'g' : 'ig'),
					resultBuilder = function (objects, start, end, parent) {
						var i, item, text;
						parent = parent || results;
						oddRow = true;
						for (i = start; i < end; i++) {
							item = $('<div></div>');
							text = objects[i].text;

							if (objects[i].items) {
								item.attr('id', 'resultGroup' + objects[i].id).addClass('resultGroup').append('<p class="text">' + text + '</p><div class="resultGroupItems"></div>');
								var items = item.find('.resultGroupItems');
								if (settings.startCollapsed) {
									items.hide();
								}
								if (settings.canCollapseGroup) {
									item.find('.text:first').click(function (resultItems) {
										return function () {
											resultItems.slideToggle();
										}
									} (items));
								}
								resultBuilder(objects[i].items, 0, objects[i].items.length, items);
							} else {
								if (settings.highlightMatches) {
									text = text.replace(filterPatt, "<strong>$1</strong>");
								}

								item.append('<p class="text">' + text + '</p>');

								if (typeof (objects[i].extra) === string) {
									item.append('<p class="extra">' + objects[i].extra + '</p>');
								}

								if (typeof (objects[i].image) === string) {
									var width = '', height = '';
									if (settings.imageSize && settings.imageSize.length) {
										if (settings.imageSize[0])
											width = settings.imageSize[0];
										if (settings.imageSize[1])
											height = settings.imageSize[1];
									}

									var imageHtml = '<img src="' + objects[i].image + '" alt=""';
									if (width != '' && width != null && width != '0') {
										imageHtml += ' width="' + width + '"';
									}
									if (height != '' && height != null && height != '0') {
										imageHtml += ' height="' + height + '"';
									}
									imageHtml += ' />';

									item.prepend(imageHtml).append('<br style="clear:both;" />');
								}

								item.addClass('resultItem').
									addClass(oddRow ? 'odd' : 'even').
									click(function (n) {
										return function () {
											selectResultItem(objects[n]);
										};
									} (i)).
									mouseover(function (el) {
										return function () {
											setHoverClass(el);
										};
									} (item));
								oddRow = !oddRow;
							}
							parent.append(item);

						}
						hasMore = typeof (settings.maxResults) == number && objects.length > end;
					};

				results.empty().hide();

				hasMore = typeof (settings.maxResults) == number && resultObjects.length > settings.maxResults;

				max = hasMore ? settings.maxResults : resultObjects.length;

				if (!resultObjects.length) {
					if (resultObjects.result !== undefined && !resultObjects.result) {
						results.append('<div class="resultItem odd timeout">' + (resultObjects.message || 'Your session has timed out, please refresh the page.') + '</div>');
					} else {
						results.append('<div class="resultItem odd">' + settings.noResults + '</div>');
					}
				} else {
					resultBuilder(resultObjects, 0, max);
					if (hasMore && settings.showMore) {
						//<a class="moreResults" href="javascript:void(0);">more</a>
						results.append($('<div class="resultItem"></div>').html(settings.more).addClass(oddRow ? 'odd' : 'even').click(function () {
							$(this).detach();
							var oldMax = max;
							max += settings.maxResults;
							if (resultObjects.length < max)
								max = resultObjects.length;
							resultBuilder(resultObjects, oldMax, max);
							if (hasMore)
								results.append(this);
						}).mouseover(function () {
							$('div.resultItem.hover', results).removeClass('hover');
							$(this).addClass('hover');
						}));
					}
				}

				if ($('div', results).length > 0) {
					currentSelection = undefined;
					showResults();

					results.css({ overflow: 'auto', maxHeight: settings.maxHeight + 'px' });
				}
			}

			function getFilterText() {
				var value = obj.val(),
					filterText = (!settings.wildCard) ? regexEscape(value) : regexEscape(value, settings.wildCard).replace(wildCardPatt, '.*');

				filterText = filterText || '.*';
				filterText = settings.wildCard ? '^' + filterText : filterText;

				if (settings.notCharacter && filterText.indexOf(settings.notCharacter) === 0) {
					filterText = filterText.substr(settings.notCharacter.length, filterText.length);
					if (filterText.length > 0) { match = false; }
				}

				return filterText;
			}

			// Prepare the search string based on the settings for this plugin,
			// run it against each item in the searchData and display any 
			// results on the page allowing selection by the user.
			function runSuggest(e) {
				var value = obj.val();
				//if (!value.length || value.length < settings.minCharacters || (obj.data('watermark') && value == obj.data('watermark'))) {
				if (!value.length || value.length < settings.minCharacters) {
					results.empty().hide();
					return false;
				}

				match = true;

				var resultObjects = [],
					ft = getFilterText(),
					fp, i, j, resultGroup;

				// Get the results from the correct place. If settings.ajaxResults then results are retrieved from
				// an external function or the server once and filtered from there else they are retrieved from the data
				// given on contruction.
				if (settings.ajaxResults === true && (typeof (searchData) === func || typeof (searchData) === string)) {
					if (typeof (searchData) === string) {
						if (!searchXHR) {
							searchingFor = value;
							results.empty().append($('<div class="resultItem odd loading"></div>').html(settings.loading)).css('height', 'auto');
							showResults();
							if (typeof (settings.data) === func) {
								searchXHR = $.getJSON(searchData, $.extend({}, settings.data(obj), { query: value, param: obj.attr('data-AccountTypeID') }), function (results) {
									var filterText = getFilterText();
									resultObjects = searchData = results;
									buildResults(resultObjects, filterText);
									searchXHR = undefined;
									if (filterText != value) {
										obj.keyup();
									}
								});
							}
							else {
								searchXHR = $.getJSON(searchData, $.extend({}, settings.data, { query: value, param: obj.attr('data-AccountTypeID') }), function (results) {
									var filterText = getFilterText();
									resultObjects = searchData = results;
									buildResults(resultObjects, filterText);
									searchXHR = undefined;
									if (filterText != value) {
										obj.keyup();
									}
								});
							}
						}
					} else {
						resultObjects = searchData = searchData(value, settings.wildCard, settings.caseSensitive, settings.notCharacter);

						if (typeof (resultObjects) === string) {
							resultObjects = JSON.parse(resultObjects);
						}
						buildResults(resultObjects, ft);
					}
				}
				else {
					fp = settings.caseSensitive ? new RegExp(ft) : new RegExp(ft, 'i');

					// Look for the required match against each single search data item. When the not
					// character is used we are looking for a false match. 
					var searchDataLen = searchData ? searchData.length : 0;
					for (i = 0; i < searchDataLen; i++) {
						if (searchData[i].items) {
							resultGroup = [];
							for (j = 0; j < searchData[i].items.length; j++) {
								if (fp.test(searchData[i].items[j].text) === match) {
									resultGroup.push(searchData[i].items[j]);
								}
							}
							if (resultGroup.length) {
								resultObjects.push($.extend({}, searchData[i], { items: resultGroup }));
							}
						}
						else if (fp.test(searchData[i].text) === match) {
							resultObjects.push(searchData[i]);
						}
					}
					buildResults(resultObjects, ft);
				}
			}

			// To call specific actions based on the keys pressed in the input
			// box. Special keys are up, down, and return. All other keys
			// act as normal.
			function keyListener(e) {
				switch (e.keyCode) {
					case 13: // enter key
						if (!currentSelection && settings.defaultToFirst) { // select first item if currentSelection is null
							currentSelection = $('div.resultItem:first', results).get(0);
						}
						//only fire 'onSelect' event if something was actually selected
						if (currentSelection) {
							$(currentSelection).click();
						}

						return false;
					case 40: // down key
						currentSelection = currentSelection ? $(currentSelection).next().get(0) : $('div.resultItem:first', results).get(0);

						setHoverClass(currentSelection);
						if (currentSelection) {
							results.scrollTop(currentSelection.offsetTop);
						}

						return false;
					case 38: // up key
						currentSelection = currentSelection ? $(currentSelection).prev().get(0) : $('div.resultItem:last', results).get(0);

						setHoverClass(currentSelection);
						if (currentSelection) {
							results.scrollTop(currentSelection.offsetTop);
						}

						return false;
					default:
						if ((this.value && this.value.length < settings.minCharacters) || (searchingFor && this.value != searchingFor)) {
							if (searchXHR) {
								searchXHR.abort();
								searchXHR = undefined;
							}
							searchData = originalSearchData;
						}
						if (settings.ajaxResults === true && (typeof (searchData) === func || typeof (searchData) === string)) {
							if (waitTimeout) {
								window.clearTimeout(waitTimeout);
							}
							waitTimeout = window.setTimeout(function () { runSuggest.apply(this, [e]); }, settings.delay);
						} else {
							runSuggest.apply(this, [e]);
						}
				}
			}

			function showResults() {
				results.css({
					top: (obj.position().top + obj.outerHeight() - 1) + 'px',
					left: obj.position().left + 'px',
					width: (settings.width || obj.width()) + 'px'
				}).show();
			}

			// Prepare the input box to show suggest results by adding in the events
			// that will initiate the search and placing the element on the page
			// that will show the results.
			results.hide();

			function handleBlur() {
				// We need to make sure we don't hide the result set
				// if the input blur event is called because of clicking on
				// a result item.
				var resPos = results.offset();
				resPos.bottom = resPos.top + results.height();
				resPos.right = resPos.left + results.width();

				var objPos = obj.offset();
				objPos.bottom = objPos.top + obj.outerHeight();
				objPos.right = objPos.left + obj.outerWidth();

				//IE doesn't count the scrollbar in the width of the div
				if ($.browser.msie)
					resPos.right += sbWidth;

				if ((pageY < resPos.top || pageY > resPos.bottom || pageX < resPos.left || pageX > resPos.right) &&
					(pageY < objPos.top || pageY > objPos.bottom || pageX < objPos.left || pageX > objPos.right)) {
					results.hide();
				}
			}

			obj.after(results).
				keyup(keyListener).
				blur(handleBlur).
				focus(function (e) {
					if ($('div', results).length > 0) {
						showResults();
					}
				}).
				attr('autocomplete', 'off');
			$(document).mousemove(function (e) {
				pageX = e.pageX;
				pageY = e.pageY;
			}).click(handleBlur);

			// Tab keyup event was not getting recognized so used keydown
			obj.keydown(function (e) {
				if (e.keyCode === 9) {
					return keyListener(e);
				}
			});

			// Opera doesn't seem to assign a keyCode for the down
			// key on the keyup event. why?
			if ($.browser.opera) {
				obj.keydown(function (e) {
					if (e.keyCode === 40) { // down key
						return keyListener(e);
					}
				});
			}

			// Escape the not character if present so that it doesn't act in the regular expression
			settings.notCharacter = regexEscape(settings.notCharacter || '');

			// We need to get the javascript array type data from the searchData setting.
			// Setting can either be a string, already an array or a function that returns one
			// of those things. We only get this data if it isn't being provided using ajax on
			// each search
			if (!settings.ajaxResults) {
				if (typeof (searchData) === func) {
					searchData = searchData();
				}
				if (typeof (searchData) === string) {
					searchData = JSON.parse(searchData);
				}
			}
		});
	};

})(jQuery);
