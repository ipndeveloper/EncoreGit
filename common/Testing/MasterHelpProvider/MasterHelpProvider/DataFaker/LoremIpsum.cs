using System;
using System.Collections.Generic;
using TestMasterHelpProvider.Extensions;

namespace TestMasterHelpProvider.DataFaker
{
    /// <summary>
    /// Helper class for demoing without full content using standard "Lorem Ipsum" sentences. - JHE
    /// </summary>
    public static class LoremIpsum
    {
        #region Enums
        public static class Enums
        {
            public enum OutputEnviroment
            {
                Windows,
                Web
            }
        }
        #endregion

        public static string GetWords(int numberOfWords, bool randomize)
        {
            string allWords = "Lorem ipsum dolor sit amet lobortis ante nisi semper nibh a dui Nulla consectetuer adipiscing elit Curabitur bibendum pede in facilisis faucibus ligula neque sit amet lectus mi iaculis in eleifend id venenatis eget pede";
            List<string> words = allWords.ToStringList(' ');

            int count = 0;
            string returnWords = string.Empty;
            while (count < numberOfWords)
            {
                if (randomize)
                    words.RandomizeList<string>();

                foreach (string word in words)
                {
                    if (count >= numberOfWords)
                        break;

                    returnWords += " " + word;
                    count++;
                }
            }
            return returnWords.Trim();
        }

        public static string GetSentences(int numberOfSentences, bool randomize)
        {
            List<string> sentences = new List<string>();
            sentences.Add("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. ");
            sentences.Add("Sed ut justo ut ligula venenatis facilisis. ");
            sentences.Add("Nam at turpis quis lectus tristique euismod. ");
            sentences.Add("Proin eleifend ipsum eu metus. ");
            sentences.Add("Phasellus eleifend metus eget libero. ");
            sentences.Add("Etiam id risus eu eros volutpat porta. ");
            sentences.Add("Nulla lectus mi, iaculis in, eleifend id, venenatis eget, pede. ");
            sentences.Add("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. ");
            sentences.Add("Donec a magna. ");
            sentences.Add("Proin sit amet pede vel quam tempus iaculis. ");
            sentences.Add("Mauris sed urna. ");
            sentences.Add("Curabitur bibendum, pede in facilisis lobortis, ante nisi semper nibh, a faucibus ligula neque sit amet dui. ");
            sentences.Add("Donec quis mauris sed leo volutpat pretium. ");
            sentences.Add("Phasellus convallis, felis eu faucibus mattis, quam ipsum ullamcorper elit, a hendrerit turpis augue id leo. ");

            int count = 0;
            string returnSentences = string.Empty;
            while (count < numberOfSentences)
            {
                if (randomize)
                    sentences.RandomizeList<string>();

                foreach (string sentence in sentences)
                {
                    if (count >= numberOfSentences)
                        break;

                    returnSentences += sentence;
                    count++;
                }
            }
            return returnSentences;
        }

        public static string GetParagraphs(int numberOfParagraphs, bool randomize)
        {
            return GetParagraphs(numberOfParagraphs, randomize, true);
        }

        public static string GetParagraphs(int numberOfParagraphs, bool randomize, bool includeParagraphTags)
        {
            return GetParagraphs(numberOfParagraphs, randomize, includeParagraphTags, Enums.OutputEnviroment.Windows);
        }

        public static string GetParagraphs(int numberOfParagraphs, bool randomize, bool includeParagraphTags, Enums.OutputEnviroment outputEnviroment)
        {
            List<string> paragraphs = new List<string>();
            paragraphs.Add("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Sed ut justo ut ligula venenatis facilisis. Nam at turpis quis lectus tristique euismod. Proin eleifend ipsum eu metus. Phasellus eleifend metus eget libero. Etiam id risus eu eros volutpat porta. Nulla lectus mi, iaculis in, eleifend id, venenatis eget, pede. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec a magna. Proin sit amet pede vel quam tempus iaculis. Mauris sed urna. Curabitur bibendum, pede in facilisis lobortis, ante nisi semper nibh, a faucibus ligula neque sit amet dui. Donec quis mauris sed leo volutpat pretium. Phasellus convallis, felis eu faucibus mattis, quam ipsum ullamcorper elit, a hendrerit turpis augue id leo. Maecenas dolor ipsum, blandit sit amet, fermentum in, pharetra vel, nisl. Morbi fermentum feugiat arcu. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. ");
            paragraphs.Add("Praesent in libero vel mauris feugiat molestie. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Cras rhoncus gravida nisl. Cras in leo pellentesque elit lobortis tempus. Maecenas sem. Vestibulum pretium, leo nec blandit convallis, tortor erat blandit elit, a tempor nibh justo at ipsum. Integer nibh risus, mollis in, sagittis in, suscipit a, enim. Ut lacinia consectetuer risus. In tempus fermentum risus. Mauris pharetra risus eu ipsum. Aliquam erat volutpat. Aenean justo magna, varius et, elementum in, mattis pulvinar, libero. Fusce cursus ipsum id ligula accumsan pretium. Nunc sollicitudin ultricies nisi. Nulla facilisi. Mauris et magna. Quisque et elit eget diam placerat tincidunt. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; ");
            paragraphs.Add("Aliquam nunc. Sed lorem orci, malesuada ut, rutrum id, mattis in, enim. Vestibulum vulputate pharetra est. Suspendisse velit massa, vulputate a, fringilla quis, tristique quis, risus. Pellentesque at urna. Sed porttitor, urna et fermentum vehicula, nulla nibh tincidunt nunc, at sagittis ante elit eget augue. Ut lobortis feugiat orci. Pellentesque turpis nunc, sagittis sit amet, vestibulum vel, aliquam at, quam. Donec felis. Aliquam ornare, magna et volutpat placerat, arcu neque ultricies risus, in egestas erat tortor et ligula. Curabitur viverra auctor magna. Ut iaculis iaculis tortor. In hac habitasse platea dictumst. Etiam condimentum tellus nec sem. ");
            paragraphs.Add("Mauris ac neque ac eros convallis pretium. Etiam vel augue. Vestibulum odio dolor, faucibus et, vestibulum nec, hendrerit ac, est. Curabitur arcu est, fermentum vel, fringilla at, sodales id, ligula. Etiam eget quam. Nullam placerat nisl sit amet nisi. Vivamus eu massa sit amet sapien tempor commodo. Integer et libero id enim malesuada rutrum. Donec odio. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Etiam dapibus. Proin id augue sit amet urna convallis tempor. ");
            paragraphs.Add("Mauris at velit. Donec hendrerit eleifend nulla. Sed in nisi. Nulla sed lorem ac lacus suscipit semper. Suspendisse elit. Donec nunc urna, pharetra non, tempor sed, facilisis ut, lectus. Quisque pretium. Fusce fringilla pharetra nunc. Cras et felis. Suspendisse eget quam. Ut sit amet justo. Morbi eget dolor vel metus consectetuer varius. Nunc aliquet. Nulla tortor. ");
            paragraphs.Add("Nunc eros odio, mattis sit amet, porta at, ornare nec, risus. Donec cursus sem quis ipsum. Sed ut sem in enim porttitor porta. Curabitur adipiscing, tortor quis tempor pulvinar, ante sapien pulvinar odio, sodales ultrices eros diam vitae eros. Etiam vel dolor. Aliquam vulputate, mauris sit amet imperdiet consequat, odio nulla malesuada arcu, ac aliquam felis felis ut magna. Aliquam scelerisque. Morbi diam eros, scelerisque fringilla, interdum id, adipiscing et, nibh. In sapien justo, hendrerit sit amet, aliquet ut, congue in, nisi. Proin metus. Sed augue. ");
            paragraphs.Add("Nulla adipiscing, nunc in vestibulum commodo, neque turpis placerat neque, eget rhoncus arcu augue at metus. Donec justo. Proin bibendum tellus sed risus. Nulla ullamcorper orci aliquam eros. Curabitur viverra mattis arcu. In elit diam, eleifend a, tempor a, dapibus a, lacus. Pellentesque dapibus vehicula lacus. Suspendisse ullamcorper tristique augue. Etiam dolor justo, ultricies eget, accumsan eget, porta sed, sem. Quisque venenatis lorem a quam. ");
            paragraphs.Add("Nullam a dui a dui adipiscing pellentesque. Pellentesque ultricies. Quisque nisl. Curabitur fringilla pharetra orci. Etiam imperdiet elit in magna. Ut quis lacus. Suspendisse potenti. Quisque et justo a massa euismod auctor. Donec purus. Ut ornare, magna quis elementum vulputate, massa turpis imperdiet leo, egestas facilisis justo ante at lacus. In hac habitasse platea dictumst. Pellentesque non ligula sit amet sem porta volutpat. Ut adipiscing neque id ipsum. Aliquam gravida, ligula pulvinar tincidunt laoreet, nulla nibh ullamcorper neque, ut lobortis nisl turpis sit amet metus. Morbi imperdiet mauris quis sapien. Ut nibh est, sollicitudin convallis, porttitor in, consequat et, augue. Nunc sodales, sem mollis tristique iaculis, pede justo consectetuer turpis, eu fermentum dolor lorem dignissim turpis. Fusce tincidunt quam et odio. Ut enim. ");
            paragraphs.Add("Integer pede odio, hendrerit non, eleifend eget, viverra eget, lacus. Praesent porttitor lacus sed orci. Mauris a nibh eget ligula dictum ultrices. Aenean non mi. Morbi dignissim. Pellentesque vel sapien. Sed lectus est, mollis in, pharetra at, semper cursus, dui. Proin eu ante sed leo venenatis tristique. Pellentesque arcu. Sed nibh. Donec faucibus urna. Sed venenatis gravida nunc. Quisque sed tellus. Etiam quis lacus sit amet augue lacinia imperdiet. Donec et tellus. Nam pellentesque sodales dolor. Quisque et arcu sed purus dignissim convallis. ");
            paragraphs.Add("Vestibulum eu ipsum in justo fermentum dapibus. Ut rhoncus. Praesent pretium, eros vel vulputate tincidunt, elit purus mattis risus, sed cursus velit nunc id lacus. Duis cursus viverra lacus. Sed id est vitae urna dictum elementum. Pellentesque adipiscing ante vitae odio. Aliquam vitae est sed erat fringilla consequat. Quisque vitae justo sit amet ante imperdiet semper. Nunc sed libero eget dolor interdum tincidunt. Nullam ullamcorper accumsan quam. Donec pretium semper libero. Fusce sodales velit eu dui. Vivamus fringilla tempus nibh. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aliquam ac nunc. Vivamus ligula mauris, porttitor non, fermentum eget, rhoncus vel, arcu. Vestibulum orci dolor, consequat non, fringilla sit amet, lacinia sed, lacus. ");
            paragraphs.Add("Etiam nisl risus, vestibulum vitae, scelerisque et, aliquet vitae, tellus. Etiam porta mauris vitae arcu. Mauris rhoncus tincidunt urna. Nullam egestas augue. Nulla vitae augue. Proin at justo. Phasellus nibh leo, volutpat vel, auctor ut, congue ut, risus. Curabitur pharetra tincidunt quam. Suspendisse potenti. Quisque lacinia mi at libero. Curabitur ut mauris. Pellentesque sed ipsum in augue lobortis luctus. ");
            paragraphs.Add("Vivamus ac leo vitae ipsum pretium lobortis. Aliquam luctus, nibh eu sodales ultricies, nibh arcu pellentesque sapien, a rhoncus quam dolor id tellus. Donec porttitor. Nulla non libero in metus semper iaculis. Suspendisse sit amet lorem vel tortor eleifend laoreet. Nam a arcu sed lectus interdum fermentum. Nullam ut sem. Nunc lobortis tempor ipsum. Sed nec sem. Cras vitae dui. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In in lorem. Etiam luctus vehicula elit. Cras hendrerit ornare lacus. Etiam imperdiet, orci eu iaculis posuere, libero eros condimentum arcu, id pulvinar ipsum risus ut est. ");

            int count = 0;
            string returnParagraphs = string.Empty;
            while (count < numberOfParagraphs)
            {
                if (randomize)
                    paragraphs.RandomizeList<string>();

                foreach (string paragraph in paragraphs)
                {
                    if (count >= numberOfParagraphs)
                        break;

                    if (includeParagraphTags)
                    {
                        if (outputEnviroment == Enums.OutputEnviroment.Web)
                            returnParagraphs += string.Format("<p>{0}</p>", paragraph);
                        else
                        {
                            //if (count + 1 < numberOfParagraphs)
                                returnParagraphs += string.Format("{0}{1}{1}", paragraph, Environment.NewLine);
                        }
                    }
                    else
                        returnParagraphs += paragraph;

                    count++;
                }
            }
            return returnParagraphs;
        }
    }
}
