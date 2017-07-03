/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license

Edited: 4/26/12 by Joey Toscano: Added PartyLite templates
Edited: 10/04/12 by Joey Toscano: Added GoldCanyon templates
*/

var url = window.location.host.split('.');

var NewTemplate = function (title, image, desc, html) {
    return { title: title, image: image, description: desc, html: html };
}

var TwoColumnNewsArticle = function () {
    return { title: 'Basic 2 Columns', image: 'twocolumn.png', description: '2 columns, no headers',
        html: '<table class="TwoColumns CKEditorTemplate">' +
                '<tr>' +
                    '<td class="ContentContentColumn1" style="width:48%;padding:10px;">Content Column 1</td>' +
                    '<td class="ContentContentColumn2" style="width:48%;padding:10px;">Content Column 2</td>' +
                '</tr>' +
              '</table><br/>&nbsp;'
    };
}
var ThreeColumn = function () {
    return { title: 'Basic 3 Column', image: 'threecolumn.png', description: '3 columns, no headers',
        html: '<table class="ThreeColumns CKEditorTemplate">' +
                '<tr>' +
                    '<td class="ContentContentColumn1" style="width:30%;padding:10px;">Content Column 1</td>' +
                    '<td class="ContentContentColumn2" style="width:30%;padding:10px;">Content Column 2</td>' +
                    '<td class="ContentContentColumn3" style="width:30%;padding:10px;">Content Column 3</td>' +
                '</tr>' +
              '</table><br/>&nbsp;'
    };
}
var PL_ImgBox1 = function () {
    return { title: '275x64 Image Box', image: 'TopImageBox.png', description: 'Product Category Top Image, Generic Template Top Image ',
        html: '<div style="width: auto; background: #fff; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); position: relative; margin-bottom: 16px;"><div class="templateBoxFullBleed" style="margin: 13px; width: 752px; height: 255px; background: #999 url(/FileUploads/CMS/Images/FPO_hero2.jpg) ;"><div class="templateBoxHeroTitle" style="position: absolute; bottom: 23px; left: 52px; color: #fff;">Section Title</div></div></div><br/>&nbsp;'
    };
}
var PL_ProductSubCat = function () {
    return { title: 'Product Subcategory Box', image: 'ProductSubCat.png', description: 'Use in product subcategory pages',
        html: '&nbsp;<br/><div class="templateBoxOuter" style="width: 778px; background: #fff; margin-bottom: 16px; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); position: relative;">' +
	'<a class="templateCatalogTab" href="/Products/Catalog"><img src="/FileUploads/CMS/Images/catalog_tab.png" /></a>' +
	'<table class="template1column" style="margin: 13px; width: 752px; border-spacing: 14px; border-collapse: separate; background: #cacaca; min-height: 30px;">' +
		'<tbody>' +
'<!--REPEATABLE LINE ITEM SNIPPET PRODUCT BEGINS-->			<tr>' +
				'<td style="background: #fff; padding: 14px;">' +
					'<img src="/FileUploads/CMS/Images/FPO_subcat_holder.jpg" style="float: left;" />' +
					'<div class="templateLineItemText">' +
						'<div class="templateLineItemTitleBar">' +
							'<div class="templateLineItemCTAs" style="position: absolute; right: 0px;">' +
								'<a href="">Link to catalog</a></div>' +
							'<div class="templateLineItemTitle">' +
								'Title</div>' +
						'</div>' +
						'<ul class="templateLineItemBody" style="margin-left: 0px; padding-left: 17px;">' +
							'<li>' +
								'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.</li>' +
							'<li>' +
								'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.</li>' +
						'</ul>' +
					'</div>' +
				'</td>' +
			'</tr>' +
'<!--REPEATABLE LINE ITEM SNIPPET PRODUCT ENDS--></tbody>' +
	'</table>' +
'</div><br/>&nbsp;'
    };
}
var PL_ProductLandingBox = function () {
    return { title: 'Product Landing Box 1', image: 'ProductLanding_Box1.png', description: 'Top image box for product landing page',
        html: '<div style="width: 780px; background: #fff; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);position: relative; margin-bottom: 16px;"><div class="templateBoxFullBleed" style="margin: 13px; width: 752px; height: 255px; background: #999 url(/FileUploads/CMS/Images/FPO_hero1.jpg);"><div class="templateBoxHeroTitle" style="position: absolute; bottom: 23px; left: 52px; color: #fff;">Section Title</div><div class="templateBoxCenterImage" style="position: absolute;  width: 226px; height: 170px; left: 50%; top: 50%; margin-left: -118px; margin-top: -85px;"><img src="/FileUploads/CMS/Images/FPO_centerplaceholder.png" /></div><div class="templateBoxHeroBlurb" style="position: absolute; right: 28px; bottom: 28px; background: #fff; padding: 14px; width: 135px;">Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh sed diam nonummy nibh.<div class="templateBoxWellCTAs"><a href="">Click Here</a></div></div></div></div><br/>&nbsp;'
    };
}
var PL_GenTemplate2_Box2 = function () {
    return { title: 'Generic Template 2 - Box 2', image: 'GenericTemplate2_Box2.png', description: 'Plain white box with text only',
        html: '<div class="templateBoxOuter" style="width: 778px;  border:1px solid #fff; background: #fff; margin-bottom: 16px;  -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);"><table class="template1column" style="margin: 13px; width: 752px; border-spacing: 14px; border-collapse: separate; background: #cacaca; min-height: 30px;"><tbody><tr><td style="background: #fff; width: 724px;"><div class="templateBoxWellText"><div class="templateBoxWellTitle">consultant testimonials</div><div class="templateBoxWellBody">Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismo tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat.</div></div></td></tr></tbody></table></div><br/>&nbsp;'
    };
}
var PL_GenTemplate2_Box3 = function () {
    return { title: 'Generic Template 2 - Box 3', image: 'GenericTemplate2_Box3.png', description: 'For big text areas with small images to the right',
        html: '<div class="templateBoxOuter" style="width: 778px;  border:1px solid #fff; background: #fff; margin-bottom: 16px;  -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);"><table class="template1column" style="margin: 13px; width: 752px; border-spacing: 14px; border-collapse: separate; background: #cacaca; min-height: 30px;"><tbody><tr><td style="background: #fff; width: 724px;"><table style="border-spacing: 14px; border-collapse: separate;"><tbody><tr><td style="background-color: #fff;"><div class="templateBoxWellTitle">Home Fragrances</div><div class="templateBoxWellBody">Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat.<p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestieconsequat.</p></div></td><td style="background-color: #fff;"><img src="/FileUploads/CMS/Images/FPO_generic_holder1.jpg" style="width: 170px; overflow: hidden; margin-top: 30px;" /> <img src="/FileUploads/CMS/Images/FPO_generic_holder2.jpg" style="width: 170px; overflow: hidden; margin-top: 30px;" /></td></tr></tbody></table></td></tr></tbody></table></div><br/>&nbsp;'
    };
}
var PL_3Col = function () {
    return { title: '3 Column Layout', image: '3Col.png', description: '3 columns with image on top, text on bottom',
        html: '<div class="templateBoxOuter" style="width: 778px; background: #fff; margin-bottom: 16px; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);">' +
	'<table class="template3column S3" style="margin: 13px; width: 752px; border-spacing: 14px; border-collapse: separate; background: #cacaca; min-height: 30px;">' +
		'<tbody>' +
			'<tr>' +
				'<td style="background: #fff; width: 232px;vertical-align:top;">' +
					'<img src="/FileUploads/CMS/Images/FPO_prod_holder2.jpg" />' +
					'<div class="templateBoxWellText">' +
						'<div class="templateBoxWellTitle">' +
							'Title</div>' +
						'<div class="templateBoxWellBody">' +
							'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat.' +
							'<div class="templateBoxWellCTAs">' +
								'<a href="">Click Here</a> <a href="">Click Here</a></div>' +
						'</div>' +
					'</div>' +
				'</td>' +
				'<td style="background: #fff;  width: 232px;vertical-align:top;">' +
					'<img src="/FileUploads/CMS/Images/FPO_prod_holder2.jpg" />' +
					'<div class="templateBoxWellText">' +
						'<div class="templateBoxWellTitle">' +
							'Title</div>' +
						'<div class="templateBoxWellBody">' +
							'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat.t.' +
							'<div class="templateBoxWellCTAs">' +
								'<a href="">Click Here</a> <a href="">Click Here</a><a href="">Click Here</a> <a href="">Click Here</a></div>' +
						'</div>' +
					'</div>' +
				'</td>' +
				'<td style="background: #fff;  width: 232px;vertical-align:top;">' +
					'<img src="/FileUploads/CMS/Images/FPO_prod_holder2.jpg" />' +
					'<div class="templateBoxWellText">' +
						'<div class="templateBoxWellTitle">' +
							'Title</div>' +
						'<div class="templateBoxWellBody">' +
							'Lorem ipsum dolor sit amet.' +
							'<div class="templateBoxWellCTAs">' +
								'<a href="">Click Here</a></div>' +
						'</div>' +
					'</div>' +
				'</td>' +
			'</tr>' +
		'</tbody>' +
	'</table>' +
'</div><br/>&nbsp;'
    };
}
var PL_3ColSplit = function () {
    return { title: '3 Column Split', image: '3ColSplit.png', description: '3 columns with the 3rd column divided vertically',
        html: '<div class="templateBoxOuter" style="width: 780px; background: #fff; margin-bottom: 16px; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);">' +
	'<table class="template3column S3" style="margin: 13px; width: 752px; border-spacing: 14px; border-collapse: separate; background: #cacaca; min-height: 30px;">' +
		'<tbody>' +
			'<tr>' +
				'<td rowspan="2" style="background: #fff; width: 232px;">' +
					'<img src="/FileUploads/CMS/Images/FPO_prod_holder.jpg" />' +
					'<div class="templateBoxWellText">' +
						'<div class="templateBoxWellTitle">' +
							'Title</div>' +
						'<div class="templateBoxWellBody">' +
							'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat.' +
							'<div class="templateBoxWellCTAs">' +
								'<a href="">Click Here</a> <a href="">Click Here</a></div>' +
						'</div>' +
					'</div>' +
				'</td>' +
				'<td rowspan="2" style="background: #fff;  width: 232px;">' +
					'<img src="/FileUploads/CMS/Images/FPO_prod_holder.jpg" />' +
					'<div class="templateBoxWellText">' +
						'<div class="templateBoxWellTitle">' +
							'Title</div>' +
						'<div class="templateBoxWellBody">' +
							'Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat.t.' +
							'<div class="templateBoxWellCTAs">' +
								'<a href="">Click Here</a> <a href="">Click Here</a><a href="">Click Here</a> <a href="">Click Here</a></div>' +
						'</div>' +
					'</div>' +
				'</td>' +
				'<td style="background: #fff;  width: 232px;">' +
					'<img src="/FileUploads/CMS/Images/FPO_prod_holder.jpg" />' +
					'<div class="templateBoxWellText">' +
						'<div class="templateBoxWellTitle">' +
							'Title</div>' +
						'<div class="templateBoxWellBody">' +
							'Lorem ipsum dolor sit amet.' +
							'<div class="templateBoxWellCTAs">' +
								'<a href="">Click Here</a></div>' +
						'</div>' +
					'</div>' +
				'</td>' +
			'</tr>' +
			'<tr>' +
				'<td style="background: #fff; width: 232px;">' +
					'<div class="templateBoxWellText">' +
						'<div class="templateBoxWellTitle">' +
							'Title</div>' +
						'<div class="templateBoxWellBody">' +
							'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.' +
							'<div class="templateBoxWellCTAs">' +
								'<a href="">Click Here</a></div>' +
						'</div>' +
					'</div>' +
				'</td>' +
			'</tr>' +
		'</tbody>' +
	'</table>' +
'</div><br/>&nbsp;'
    };
}
var PL_4Col = function () {
    return { title: '4 Column Layout', image: '4Col.png', description: '3 columns with image on top, text on bottom',
        html: '<div class="templateBoxOuter" style="width: 778px; background: #fff; margin-bottom: 16px; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);">' +
	'<table class="template3column S3" style="margin: 13px; width: 752px; border-spacing: 14px; border-collapse: separate; background: #cacaca; min-height: 30px;">' +
		'<tbody>' +
			'<tr>' +
				'<td style="background: #fff; width: 171px;">' +
					'<img src="/FileUploads/CMS/Images/FPO_test_holder1.jpg"  style="width: 173px; overflow: hidden;" />' +
					'<div class="templateBoxWellText">' +
						'<a href="" class="templateBoxWellTitle">' +
							'Become a consultant</div>' +
					'</div>' +
				'</td>' +
				'<td style="background: #fff;  width:172px;">' +
					'<img src="/FileUploads/CMS/Images/FPO_test_holder2.jpg"  style="width: 173px; overflow: hidden;" />' +
					'<div class="templateBoxWellText">' +
						'<a href=""  class="templateBoxWellTitle">' +
							'Consultant testimonials</div>' +
					'</div>' +
				'</td>' +
			'<td style="background: #fff;  width: 172px;">' +
					'<img src="/FileUploads/CMS/Images/FPO_test_holder3.jpg"  style="width: 173px; overflow: hidden;" />' +
					'<div class="templateBoxWellText">' +
						'<a href=""  class="templateBoxWellTitle">' +
							'Consultant Rewards</div>' +
					'</div>' +
				'</td>' +
				'<td style="background: #fff;  width: 171px;">' +
					'<img src="/FileUploads/CMS/Images/FPO_test_holder4.jpg" style="width: 173px; overflow: hidden;" />' +
					'<div class="templateBoxWellText">' +
						'<a href=""  class="templateBoxWellTitle">' +
							'Start Now</div>' +
					'</div>' +
				'</td>' +
			'</tr>' +
		'</tbody>' +
	'</table>' +
'</div><br/>&nbsp;'
    };
}
var PL_5Col = function () {
    return { title: '5 Column Layout', image: '5Col.png', description: '5 columns with image on top, text on bottom',
        html: '<div class="templateBoxOuter" style="width: 778px; background: #fff; margin-bottom: 16px; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);"><table class="template3column S3" style="margin: 13px; width: 752px; border-spacing: 14px; border-collapse: separate; background: #cacaca; min-height: 30px; table-layout:fixed; along with overflow:hidden;"><tbody><tr><td style="background: #fff;  width:133px;"><img src="/FileUploads/CMS/Images/FPO_test_holder1.jpg" style="width: 133px; overflow: hidden;" /><div class="templateBoxWellText"><a class="templateBoxWellTitle" href="">The Team</a></div></td><td style="background: #fff;  width:134px;"><img src="/FileUploads/CMS/Images/FPO_test_holder2.jpg" style="width: 134px; overflow: hidden;" /><div class="templateBoxWellText"><a class="templateBoxWellTitle" href="">Vision</a></div></td><td style="background: #fff;  width: 134px;"><img src="/FileUploads/CMS/Images/FPO_test_holder3.jpg" style="width: 134px; overflow: hidden;" /><div class="templateBoxWellText"><a class="templateBoxWellTitle" href="">Team</a></div></td><td style="background: #fff;  width: 134px;"><img src="/FileUploads/CMS/Images/FPO_test_holder4.jpg" style="width: 134px; overflow: hidden;" /><div class="templateBoxWellText"><a class="templateBoxWellTitle" href="">Direct Selling</a></div></td><td style="background: #fff;  width: 133px;"><img src="/FileUploads/CMS/Images/FPO_test_holder4.jpg" style="width: 133px; overflow: hidden;" /><div class="templateBoxWellText"><a class="templateBoxWellTitle" href="">Worldwide Locations</a></div></td></tr></tbody></table></div><br/>&nbsp;'
    };
}
var PL_BigImgTxtRight = function () {
    return { title: 'Big Image Left, Text Right', image: 'BigImageLeft_TextRight.png', description: 'Confirmation pages, simple forms, etc',
        html: '<div style="width: 778px; background: #fff; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); position: relative; margin-bottom: 16px;"><table style="border-spacing: 14px; border-collapse: separate;"><tbody><tr><td><div style="width: 496px; height: 331px; background: #999 url(/FileUploads/CMS/Images/FPO_hostess.jpg) ; overflow: hidden;">&nbsp;</div><div class="templateBoxHeroTitle" style="position: absolute; bottom: 23px; left: 52px; color: #fff;">Become a Host</div></td><td>Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.</td></tr></tbody></table></div><br/>&nbsp;'
    };
}
var PL_TopImgTextOverlay = function () {
    return { title: 'Big Image w/ Overlay Text Box', image: 'BigTopImage_OverlayTextBox.png', description: 'Categegory Landing',
        html: '<div style="width: 778px; background: #fff; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); position: relative; margin-bottom: 16px;"><div class="templateBoxFullBleed" style="margin: 13px; width: 752px; height: 255px; background: #999 url(/FileUploads/CMS/Images/FPO_hero2.jpg) ;"><div class="templateBoxHeroQuote" style="position: absolute; left: 43px; top: 61px; background: #e1e0d9; padding: 17px; width: 300px;"><div class="templateBoxHeroQuoteBlurb">&ldquo;Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam...&rdquo;</div><div class="templateBoxWellCTAs" style="margin-top: 5px;"><a class="templateBoxQuoteCTA" href="">Testimonials</a></div></div></div></div><br/>&nbsp;'
    };
}
var PL_TopImgTextOverlayTopLeft = function () {
    return { title: 'Big Image w/ Overlay 2', image: 'BigTopImage_OverLayTextTopLeft.png', description: 'Big image with text overlay in top left',
        html: '<div style="width: 778px; background: #fff; border:1px solid #fff; -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); position: relative; margin-bottom: 16px;"><div class="templateBoxFullBleed" style="margin: 13px; width: 752px; height: 255px; background: #999 url(/FileUploads/CMS/Images/FPO_hero2.jpg) ;"><div class="templateBoxHeroQuote" style="position: absolute; left: 43px; top: 61px; background: #e1e0d9; padding: 17px; width: 300px;"><div class="templateBoxHeroQuoteBlurb">&ldquo;Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam...&rdquo;</div><div class="templateBoxWellCTAs" style="margin-top: 5px;"><a class="templateBoxQuoteCTA" href="">Testimonials</a></div></div></div></div><br/>&nbsp;'
    };
}
var PL_2x2 = function () {
    return { title: '2 x 2', image: '2x2.png', description: '2 shadow boxes, side-by-side',
        html: '<div class="templateBoxOuter templateBoxLeft" style="width: 381px;  border:1px solid #fff; background: #fff; margin-bottom: 16px;  -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); float: left;"><table class="template1column" style="margin: 13px; width: 355px; border-spacing: 14px; border-collapse: separate; background: #cacaca;"><tbody><tr><td style="background: #fff; width: 327px; height: 360px;"><img src="/FileUploads/CMS/Images/FPO_prod_holder3.jpg" /><div class="templateBoxWellText"><div class="templateBoxWellTitle">Title</div><div class="templateBoxWellBody">Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat.<div class="templateBoxWellCTAs"><a href="">Click Here</a></div></div></div></td></tr></tbody></table></div><div class="templateBoxOuter templateBoxRight" style="width: 381px; border:1px solid #fff; background: #fff; margin-bottom: 16px;  -moz-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); -webkit-box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15);box-shadow: 7px 7px 7px rgba(0, 0, 0, 0.15); float: right;"><table class="template1column" style="margin: 13px; width: 355px; border-spacing: 14px; border-collapse: separate; background: #cacaca;"><tbody><tr><td style="background: #fff; width: 327px; height: 360px;"><img src="/FileUploads/CMS/Images/FPO_prod_holder4.jpg" /><div class="templateBoxWellText"><div class="templateBoxWellTitle">Title</div><div class="templateBoxWellBody">Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonumputate velit esse molestie consequat.<div class="templateBoxWellCTAs"><a href="">Click Here</a></div></div></div></td></tr></tbody></table></div><br/>&nbsp;'
    };
}



// Basic Image Slide Show
var ImgSlideShow = function () {
    return { title: 'Image-Only Slide Show', image: 'BasicSlideShow.png', description: 'Image-only slide show. Images automatically scroll horizontally, or user can click a button to pause the slideshow.  DO NOT USE MORE THAN ONE OF THESE ON A PAGE.',
        html: '<div id="NewsSlideWrap">' +
	'<div id="NewsSlideInnerWrap">' +
		'<div class="slideGroup">' +        
			'<div class="slide">' +
				'<a href="#"><img src="https://resources.netsteps.com/ckeditor/3.5.2/images/spacer.gif" style="width: 100%; height: 366px;background:#333;"/></a></div>' +     
			'<div class="slide">' +
				'<a href="#"><img src="https://resources.netsteps.com/ckeditor/3.5.2/images/spacer.gif" style="width: 100%; height: 366px;background:#777;"/></a></div>' +
            '<div class="slide">' +
				'<a href="#"><img src="https://resources.netsteps.com/ckeditor/3.5.2/images/spacer.gif" style="width: 100%; height: 366px;background:#333;"/></a></div>' +
            '<div class="slide">' +
				'<a href="#"><img src="https://resources.netsteps.com/ckeditor/3.5.2/images/spacer.gif" style="width: 100%; height: 366px;background:#777;"/></a></div>' +       
		'</div>' +
'</div></div>' +
        '&nbsp;'
    };
}



// Rendi Templates
if ((url[1].search(/rendi/i) != -1 || url[0].search(/rendi/i) != -1)) {
    CKEDITOR.addTemplates('default', {
        imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/'),
        templates:
        [
            NewTemplate('Year In Pictures (Right Image)', 'Rendi/tiltimgtoright.png', 'Tilted image to the RIGHT of some text.',
                        '<img style="margin:-60px 50px 30px 20px; float:right; transform: rotate(10deg); -webkit-transform: rotate(10deg); -moz-transform: rotate(10deg);" src="/FileUploads/CMS/Images/fpo.png">' +
                        '<p><span style="font-weight:bold; font-style:italic;">Double click the image to replace it with your uploaded image, and to edit the image information.  Then, replace this text with your text.</p>' +
                        '<span class="clrall">&nbsp;</span>'),

            NewTemplate('Year In Pictures (Left Image)', 'Rendi/tiltimgtoleft.png', 'Tilted image to the LEFT of some text.',
                        '<img style="margin:-60px 20px 30px 50px; float:left; transform: rotate(-10deg); -webkit-transform: rotate(-10deg); -moz-transform: rotate(-10deg);" src="/FileUploads/CMS/Images/fpo.png">' +
                        '<p><span style="font-weight:bold; font-style:italic;">Double click the image to replace it with your uploaded image, and to edit the image information.  Then, replace this text with your text.</p>' +
                        '<span class="clrall">&nbsp;</span>'),

            NewTemplate('Float Right Image Content Column with Header', 'Rendi/imgtoright.png', 'Header that spans the page with a padded content column and image floated right',
                        '<table class="FloatRightImageContentColumn">' +
                            '<tr>' +
                                '<th>' +
                                    '<h1 style="margin:0 0 20px;" class="contentHeader">Image Float Right Template</h1>' +
                                '</th>' +
                            '</tr>' +
                            '<tr>' +
                                '<td class="contentBody">' +
                                    '<img style="float:right;margin:5px 0 10px 20px;" class="contentImage" src="/FileUploads/CMS/Images/fpo.png" />' +
                                    '<br />' +
                                    'Double click the image to replace it with your uploaded image, and to edit the image information. Then, replace this text with your text.' +
                                    '<br />' +
                                '</td>' +
                            '</tr>' +
                        '</table><br/>&nbsp;'),

            NewTemplate('Float Left Image Content Column with Header', 'Rendi/imgtoleft.png', 'Header that spans the page with a padded content column and image floated left',
                        '<table class="FloatLeftImageContentColumn">' +
                            '<tr>' +
                                '<th>' +
                                    '<h1 style="margin:0 0 20px;" class="contentHeader">Image Float Right Template</h1>' +
                                '</th>' +
                            '</tr>' +
                            '<tr>' +
                                '<td class="contentBody">' +
                                    '<img style="float:right;margin:5px 20px 10px 0;" class="contentImage" src="/FileUploads/CMS/Images/fpo.png" />' +
                                    '<br />' +
                                    'Double click the image to replace it with your uploaded image, and to edit the image information. Then, replace this text with your text.' +
                                    '<br />' +
                                '</td>' +
                            '</tr>' +
                        '</table>'),
            NewTemplate('Product Grid Framework', 'Rendi/frametemplate.png', 'This is the base framework for the products grid and should be used before you start entering in produts with the Products Grid template',
                        '<table>' +
                            '<tr>' +
                                '<th colspan="2">' +
                                    '<h1 style="margin:0 0 20px;">Product Grid Framework</h1>' +
                                '</th>' +
                            '</tr>' +
                            '<tr>' +
                                '<td colspan="2">' +
                                    'Add a description of the page in this block' +
                                '</td>' +
                            '</tr>' +
                            '<tr>' +
                                '<td class="ProductGrid">' +
                                    'Remove this text and use the Products Grid template' +
                                '</td>' +
                            '</tr>' +
                        '</table>'),
            NewTemplate('Products Grid', 'Rendi/innertemplate.png', 'Products grid for the Product Grid Framework. This also includes the right details column',
                    '<table><tr><td>' +
                        '<table>' +
                            '<tr>' +
                                '<td style="width:250px;">' +
                                    '<a href="#" style="display:block;text-align:center;">' +
                                        '<img src="/FileUploads/CMS/Images/fpo.png" style="max-width:250px;max-height:250px;" />' +
                                        '<strong style="color:#1f7a72;display:block;text-align:center;margin:5px 0 20px;">' +
                                            'Product Name<br />' +
                                            '<span style="color:#000;">Item # R0000: <span style="text-decoration:line-through;">$OldPrice</span> <span style="color:#900;">$NewPrice!</span></span>' +
                                        '</strong>' +
                                    '</a>' +
                                '</td>' +
                                '<td style="width:250px;">' +
                                    '<a href="#" style="display:block;text-align:center;">' +
                                        '<img src="/FileUploads/CMS/Images/fpo.png" style="max-width:250px;max-height:250px;" />' +
                                        '<strong style="color:#1f7a72;display:block;text-align:center;margin:5px 0 20px;">' +
                                            'Product Name<br />' +
                                            '<span style="color:#000;">Item # R0000: <span style="text-decoration:line-through;">$OldPrice</span> <span style="color:#900;">$NewPrice!</span></span>' +
                                        '</strong>' +
                                    '</a>' +
                                '</td>' +
                            '</tr>' +
                        '</table>' +
                    '</td>' +
                    '<td style="background:url(/FileUploads/CMS/Images/Brace_BKD.png) repeat-y left top;padding:0;">' +
                        '<div class="ProductDescription" style="position:relative;padding:0;border-left:solid 1px #999;width:200px;height:100%;padding:0 0 0 20px;">' +
                            '<div style="background:url(/FileUploads/CMS/Images/Brace_T.png) no-repeat left top;width:25px;height:25px;position:absolute;top:0;left:-1px;"></div>' +
                            '<div style="background:url(/FileUploads/CMS/Images/Brace_M.png) no-repeat left center;height:100%;margin:0 0 0 -38px;padding:0 0 0 50px;">' +
                                '<div class="RightColumnContent" style="position:relative;">' +
                                    'Right column details' +
                                '</div>' +
                            '</div>' +
                            '<div style="background:url(/FileUploads/CMS/Images/Brace_B.png) no-repeat left bottom;width:25px;height:25px;position:absolute;bottom:0;left:-1px;"></div>' +
                        '</div>' +
                    '</td></tr></table><br/>&nbsp;'),
        //NewsArticleWithHeaderAndSubHeader(),
        //TwoColumnWithHeaders(),
            TwoColumnNewsArticle(),
            ThreeColumn()
        ]
    });
}
// PartyLite
else if ((url[1].search(/PartyLite/i) != -1 || url[0].search(/PartyLite/i) != -1)) {
    CKEDITOR.addTemplates('default', {
        imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/PartyLite/'),
        templates:
        [
            PL_ImgBox1(),
            PL_ProductSubCat(),
            PL_ProductLandingBox(),
            PL_GenTemplate2_Box2(),
            PL_GenTemplate2_Box3(),
            PL_3Col(),
            PL_3ColSplit(),
            PL_4Col(),
            PL_5Col(),
            PL_BigImgTxtRight(),
            PL_TopImgTextOverlay(),
            PL_TopImgTextOverlayTopLeft(),
            PL_2x2()
        ]
    });
}
// GoldCanyon Corp
else if ((url[1].search(/goldcanyon/i) != -1 || url[0].search(/goldcanyon/i) != -1)) {
    CKEDITOR.addTemplates('default', {
        imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/'),
        templates:
        [
            TwoColumnNewsArticle(),
            ThreeColumn(),
            ImgSlideShow()
        ]
    });
}

// GoldCanyon PWS
else if ((url[1].search(/mygc/i) != -1 || url[0].search(/mygc/i) != -1)) {
    CKEDITOR.addTemplates('default', {
        imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/'),
        templates:
        [
            TwoColumnNewsArticle(),
            ThreeColumn(),
            ImgSlideShow()
        ]
    });
}



// Default Templates
else {
    CKEDITOR.addTemplates('default', {
        imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/'),
        templates:
        [
        //NewsArticleWithHeaderAndSubHeader(),
        //TwoColumnWithHeaders(),
            TwoColumnNewsArticle(),
            ThreeColumn()
        ]
    });
}