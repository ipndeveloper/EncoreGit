Author - Daniel Stafford
Date Created - 9/8/2010
Last Updated - 9/8/2010

AccountTypes
-------------
We have predefined 3 account types that are supported: Retail Customer, Preferred Customer, and Distributor.  This value coincides with the NetSteps.Data.Entities.Constants.AccountType enumeration.
You can enable or disable account types with the Enabled attribute.
Each account type has 2 sub nodes: AccountInfo and AddOns

AccountInfo
-----------
This node is to tell the enrollment which control to use to gather information about the account

AddOns
------
This node is the parent for a collection of AddOn nodes, which define the steps of the enrollment process

AddOn
-----
Name: A user friendly name for when the enrollment config has a front-end to it
UseControl: Tells the enrollment which control in the Areas/Enrollment/Views/Shared directory to use
Skippable: Allow the user to skip this addon (must be handled in the addon itself, see any of the default addons for an example)



Controls
--------
This node stores all of the controls that are used in the enrollment, and also any properties associated with that control

Control
-------
Name: The name of the control, to load the control, and to be used with the UseControl attribute on AccountInfo and AddOn

Properties
----------
A container node to hold all of the properties of a given control

Property
--------
Name: A string based lookup for when you are loading properties into a control
The contents of this node can be anything, it must be handled by the calling code



The following is a very small example of a simple 2 step enrollment process:
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<Enrollment>
	<AccountTypes>
		<AccountType Name="PreferredCustomer" Enabled="true">
			<AccountInfo UseControl="PreferredCustomerInfo" />
			<AddOns>
				<AddOn Name="Autoship" UseControl="Autoship" Skippable="true" />
			</AddOns>
		</AccountType>
	</AccountTypes>
	<Controls>
		<Control Name="PreferredCustomerInfo" />

		<Control Name="Autoship">
			<Properties>
				<Property Name="PreBuiltAutoships">
					<Autoship Name="Pre-built Autoship 1" AutoshipScheduleID="1">
						<Description><![CDATA[This is an awesome autoship that distributors sign up for]]></Description>
						<Products>
							<Product SKU="DAN-AWESOME" Quantity="1" />
						</Products>
					</Autoship>
				</Property>
				<Property Name="MinimumCV">0</Property>
			</Properties>
		</Control>
	</Controls>
</Enrollment>




-----------------------------------------------------
                 Pre-built Controls
-----------------------------------------------------

Site Subscriptions
------------------
Properties:
    BaseSiteID - The ID of the base site to use when signing up new accounts
	AutoshipSchedules - The available schedules for site subscriptions (content are XML in the format <AutoshipSchedule ID="3" />)

Autoship
--------
Properties:
	PreBuiltAutoships - A collection of recommended autoships (contents are a collection of Autoship XML nodes)
	    Autoship: Defines the name and the Autoship Schedule ID (i.e. <Autoship Name="Pre-built Autoship 1" AutoshipScheduleID="1">)
		    Description: Description of the prebuilt autoship
			Products: A collection of the products that go on this Autoship
			    Product: Defines the SKU and the quantity to go on the autoship (i.e. <Product SKU="DAN-AWESOME" Quantity="1" />)
	MinimumCV - The minimum commissionable value that an autoship can have (set to 0 if there is no minimum)

Initial Order
-------------
Properties:
    OrderType - Fixed (cannot add or remove items), FixedAndVariable (has some fixed items, but can add or remove other items), or Variable (can add or remove any item)
	Products - The products that go on a fixed order
	    Products: The container for all of the products
		    Product: Defines the SKU and the quantity that goes on the order (i.e. <Product SKU="DAN-AWESOME" Quantity="1" />)
    MinimumCV - The minimum commissionable value that an autoship can have (set to 0 if there is no minimum)

Disbursement Profiles
--------------------
Properties:
    EnableCheckProfile - true or false, determines whether check profile is available
	EnableEFTProfile - true or false, determines whether EFT profiles are available