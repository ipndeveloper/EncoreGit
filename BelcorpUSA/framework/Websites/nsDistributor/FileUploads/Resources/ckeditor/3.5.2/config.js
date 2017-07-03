/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	config.toolbar_CMS = 
	[
		['Source','-','Save','NewPage','Preview','-','Templates'],
		['Cut','Copy','Paste','PasteText','PasteFromWord','-','Print', 'SpellChecker', 'Scayt'],
		['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
		['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],
		['BidiLtr', 'BidiRtl'],
		'/',
		['Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
		['NumberedList','BulletedList','-','Outdent','Indent','Blockquote','CreateDiv'],
		['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
		['Link','Unlink','Anchor'],
		['Image','Flash','Table','HorizontalRule','Smiley','SpecialChar','PageBreak'],
		'/',
		['Styles','Format','Font','FontSize'],
		['TextColor','BGColor'],
		['Maximize', 'ShowBlocks']
	];
	
	config.toolbar_NetSteps =
 	[
       		['Cut','Copy','Paste','PasteText','PasteFromWord','-','Scayt'],
       		['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
       		['Table', 'HorizontalRule', 'SpecialChar', 'PageBreak'],
       		'/',
       		['Styles','Format'],
       		['Bold','Italic','Strike'],
       		['NumberedList','BulletedList','-','Outdent','Indent'],
       		['Link','Unlink','Anchor'],
       		['Maximize', '-', ], ['Source', '-', ]
    	];


	config.extraPlugins = 'mediaembed,nsjsvideo';
	config.stylesSet = 'Custom_Encore_Styles:encore_styles.js';

};





