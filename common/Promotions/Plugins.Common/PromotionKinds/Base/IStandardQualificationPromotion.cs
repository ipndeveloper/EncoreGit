using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Base
{
    [ContractClass(typeof(StandardQualificationPromotionContract))]
    public interface IStandardQualificationPromotion : IPromotion
    {


        /// <summary>
        /// Gets a value indicating whether the promotion requires entry of a promotion code.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the promotion requires a promotion code; otherwise, <c>false</c>.
        /// </value>
        bool RequiresPromotionCode { get; }

        /// <summary>
        /// Gets or sets the promotion code.
        /// </summary>
        /// <value>The promotion code.</value>
        string PromotionCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an account may only use this promotion once..
        /// </summary>
        /// <value>
        ///     <c>true</c> if the promotion is restricted to a continuity; otherwise, <c>false</c>
        /// </value>
        bool Continuity { get; set; } //INI-FIN - GR_Encore-07

        /// <summary>
        /// Gets or sets a value indicating whether an account may only use this promotion once..
        /// </summary>
        /// <value>
        ///   <c>true</c> if the promotion is restricted to a single use; otherwise, <c>false</c>.
        /// </value>
        bool OneTimeUse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an account may only use this promotion once..
        /// </summary>
        /// <value>
        ///   <c>true</c> if the promotion is restricted to a single use on first orders only; otherwise, <c>false</c>.
        /// </value>
        bool FirstOrdersOnly { get; set; }

        /// <summary>
        /// Gets the restricted to account titles or types.
        /// </summary>
        /// <value>The restricted to account titles or types.</value>
        bool RestrictedToAccountTitlesOrTypes { get; }

        /// <summary>
        /// Gets the account title ids allowed when the promotion is restricted by account ID; empty if not restricted
        /// </summary>
        IEnumerable<IAccountTitleOption> AccountTitles { get; }

        /// <summary>
        /// Gets the account consistency status IDs.
        /// </summary>
        /// <value>The account consistency status IDs.</value>
        IEnumerable<short> AccountConsistencyStatusIDs { get; }

        /// <summary>
        /// Gets the activity status IDs.
        /// </summary>
        /// <value>The activity status IDs.</value>
        IEnumerable<short> ActivityStatusIDs { get; }

        /// <summary>
        /// Gets the account type IDs.
        /// </summary>
        /// <value>The account type IDs.</value>
        IEnumerable<short> AccountTypeIDs { get; }

        /// <summary>
        /// Gets the account IDs.
        /// </summary>
        /// <value>The account IDs.</value>
        IEnumerable<int> AccountIDs { get; }

        /// <summary>
        /// Gets a value indicating whether this promotion is restricted to specific order types.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the promotion is restricted by order type; otherwise, <c>false</c>.
        /// </value>
        bool RestrictedToOrderTypes { get; }

        /// <summary>
        /// Specifies which order types are allowed when it is restricted; empty if not restricted.
        /// </summary>
        IEnumerable<int> OrderTypes { get; }

        /// <summary>
        /// Adds the account title ID.
        /// </summary>
        /// <param name="accountTitleID">The account title ID.</param>
        /// <param name="titleTypeID">The title type ID.</param>
        void AddAccountTitle(int accountTitleID, int titleTypeID);

        /// <summary>
        /// Deletes the account title ID.
        /// </summary>
        /// <param name="accountTitleID">The account title ID.</param>
        void DeleteAccountTitle(int accountTitleID, int titleTypeID);

        /// <summary>
        /// Adds the order type ID.
        /// </summary>
        /// <param name="orderTypeID">The order type ID.</param>
        void AddOrderTypeID(int orderTypeID);

        /// <summary>
        /// Deletes the order type ID.
        /// </summary>
        /// <param name="orderTypeID">The order type ID.</param>
        void DeleteOrderTypeID(int orderTypeID);

        /// <summary>
        /// Adds the order type ID.
        /// </summary>
        /// <param name="accountConsistencyStatusID">The account consistency status ID.</param>
        void AddAccountConsistencyStatusID(short accountConsistencyStatusID); /*R3144*/

        /// <summary>
        /// Deletes the order type ID.
        /// </summary>
        /// <param name="accountConsistencyStatusID">The account consistency status ID.</param>
        void DeleteAccountConsistencyStatusID(short accountConsistencyStatusID);

        /// <summary>
        /// Adds the order type ID.
        /// </summary>
        /// <param name="activityStatusID">The activity status ID.</param>
        void AddActivityStatusID(short activityStatusID);

        /// <summary>
        /// Deletes the order type ID.
        /// </summary>
        /// <param name="activityStatusID">The activity status ID.</param>
        void DeleteActivityStatusID(short activityStatusID);

        /// <summary>
        /// Adds the account type ID.
        /// </summary>
        /// <param name="accountTypeID">The account type ID.</param>
        void AddAccountTypeID(short accountTypeID);

        /// <summary>
        /// Deletes the account type ID.
        /// </summary>
        /// <param name="accountTypeID">The account type ID.</param>
        void DeleteAccountTypeID(short accountTypeID);

        /// <summary>
        /// Adds the account  ID.
        /// </summary>
        /// <param name="accountID">The account  ID.</param>
        void AddAccountID(int accountID);

        /// <summary>
        /// Deletes the account ID.
        /// </summary>
        /// <param name="accountID">The account  ID.</param>
        void DeleteAccountID(int accountID);
    }

    [ContractClassFor(typeof(IStandardQualificationPromotion))]
    public abstract class StandardQualificationPromotionContract : IStandardQualificationPromotion
    {

        public bool RequiresPromotionCode
        {
            get { return false; }
        }

        public string PromotionCode { get; set; }

        public bool Continuity { get; set; } //INI-FIN - GR_Encore-07

        public bool OneTimeUse { get; set; }

        public bool FirstOrdersOnly { get; set; }

        public bool RestrictedToAccountTitlesOrTypes { get; set; }

        public IEnumerable<IAccountTitleOption> AccountTitles
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IAccountTitleOption>>() != null);
                throw new NotImplementedException();
            }
        }

        public bool RestrictedToOrderTypes
        {
            get { return false; }
        }

        public IEnumerable<short> AccountTypeIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<short>>() != null);
                return new List<short>();
            }
        }

        //INI - GR_Encore-07
        public IEnumerable<short> AccountConsistencyStatusIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<short>>() != null);
                return new List<short>();
            }
        }

        public IEnumerable<short> ActivityStatusIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<short>>() != null);
                return new List<short>();
            }
        }
        //FIN - GR_Encore-07

        public IEnumerable<int> AccountIDs
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<int>>() != null);
                return new List<int>();
            }

        }

        public IEnumerable<int> OrderTypes
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<int>>() != null);
                return new List<int>();
            }
        }

        public void AddAccountTitle(int accountTitleID, int titleTypeID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(accountTitleID > 0);
        }

        public void DeleteAccountTitle(int accountTitleID, int titleTypeID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(accountTitleID > 0);
        }

        public void AddProductID(int productID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(productID > 0);
        }

        public void DeleteProductID(int productID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(productID > 0);
        }

        //INI - GR_Encore-07
        public void AddAccountConsistencyStatusID(short accountConsistencyStatusID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(accountConsistencyStatusID > 0);
        }

        public void DeleteAccountConsistencyStatusID(short accountConsistencyStatusID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(accountConsistencyStatusID > 0);
        }

        public void AddActivityStatusID(short activityStatusID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(activityStatusID > 0);
        }

        public void DeleteActivityStatusID(short activityStatusID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(activityStatusID > 0);
        }
        //FIN - GR_Encore-07

        public void AddOrderTypeID(int orderTypeID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(orderTypeID > 0);
        }

        public void DeleteOrderTypeID(int orderTypeID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(orderTypeID > 0);
        }

        public void AddAccountTypeID(short accountTypeID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(accountTypeID > 0);
        }

        public void DeleteAccountTypeID(short accountTypeID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(accountTypeID > 0);
        }

        public void AddAccountID(int accountID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(accountID > 0);
        }

        public void DeleteAccountID(int accountID)
        {
            Contract.Requires<ArgumentOutOfRangeException>(accountID > 0);
        }

        public abstract IEnumerable<string> AssociatedPropertyNames { get; }

        public abstract string Description { get; set; }

        public abstract DateTime? EndDate { get; set; }

        public abstract int PromotionID { get; set; }

        public abstract string PromotionKind { get; }

        public abstract IDictionary<string, IPromotionQualificationExtension> PromotionQualifications { get; set; }

        public abstract IDictionary<string, IPromotionReward> PromotionRewards { get; set; }

        public abstract int PromotionStatusTypeID { get; set; }

        public abstract DateTime? StartDate { get; set; }

        public abstract bool ValidFor<TProperty>(string propertyName, TProperty value);
    }
}
