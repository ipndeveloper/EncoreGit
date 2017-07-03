using System.Collections.Generic;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.DataFaker
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Methods to generate 'Fake' data for use in testing.
	/// Created: 03-18-2009
	/// </summary>
	public static class NameFaker
	{
		private static List<string> _lastNames = "Abel, Anderson, Andrews, Anthony, Baker, Brown, Burrows, Clark, Clarke, Clarkson, Davidson, Davies, Davis, Dent, Edwards, Garcia, Grant, Hall, Harris, Harrison, Jackson, Jeffries, Jefferson, Johnson, Jones, Kirby, Kirk, Lake, Lee, Lewis, Martin, Martinez, Major, Miller, Moore, Oates, Peters, Peterson, Robertson, Robinson, Rodriguez, Smith, Smythe, Stevens, Taylor, Thatcher, Thomas, Thompson, Walker, Washington, White, Williams, Wilson, Yorke".ToStringList();
		private static List<string> _femaleFirstNames = "Alison, Ann, Anna, Anne, Barbara, Betty, Beryl, Carol, Charlotte, Cheryl, Deborah, Diana, Donna, Dorothy, Elizabeth, Eve, Felicity, Fiona, Helen, Helena, Jennifer, Jessica, Judith, Karen, Kimberly, Laura, Linda, Lisa, Lucy, Margaret, Maria, Mary, Michelle, Nancy, Patricia, Polly, Robyn, Ruth, Sandra, Sarah, Sharon, Susan, Tabitha, Ursula, Victoria, Wendy".ToStringList();
		private static List<string> _maleFirstNames = "Adam, Anthony, Arthur, Brian, Charles, Christopher, Daniel, David, Donald, Edgar, Edward, Edwin, George, Harold, Herbert, Hugh, James, Jason, John, Joseph, Kenneth, Kevin, Marcus, Mark, Matthew, Michael, Paul, Philip, Richard, Robert, Roger, Ronald, Simon, Steven, Terry, Thomas, William".ToStringList();

		public static string LastName()
		{
			return _lastNames.GetRandom().ToCleanString();
		}

		public static string FirstName()
		{
			return Random.GetBoolean() ? FemaleFirstName() : MaleFirstName();
		}

		public static string FemaleFirstName()
		{
			return _femaleFirstNames.GetRandom().ToCleanString();
		}

		public static string MaleFirstName()
		{
			return _maleFirstNames.GetRandom().ToCleanString();
		}

		public static string MaleName()
		{
			return MaleFirstName() + " " + LastName();
		}

		public static string FemaleName()
		{
			return FemaleFirstName() + " " + LastName();
		}

		public static string Name()
		{
			return FirstName() + " " + LastName();
		}

		public static Name NameObject()
		{
			Name name = new Name();
			name.IsMale = Random.GetBoolean();
			name.FirstName = (name.IsMale) ? MaleFirstName() : FemaleFirstName();
			name.MiddleName = (name.IsMale) ? MaleFirstName() : FemaleFirstName();
			name.LastName = LastName();
			return name;
		}
	}

	public class Name
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string FirstAndLastName
		{
			get
			{
				return string.Format("{0} {1}", FirstName, LastName);
			}
		}
		public string FullName
		{
			get
			{
				return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
			}
		}
		public bool IsMale { get; set; }
	}
}
