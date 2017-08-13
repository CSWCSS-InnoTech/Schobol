using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InnoTecheLearning
{
    [AcceptEmptyServiceProvider]
    [ContentProperty(nameof(Columns))]
    public class GridColumnExtension : IMarkupExtension<ColumnDefinitionCollection>
    {
        public string Columns { get; set; }

        public ColumnDefinitionCollection ProvideValue(IServiceProvider serviceProvider)
        {
            var Return = new ColumnDefinitionCollection();
            var Columns = this.Columns.Split(',');
            var GridLengths = new GridLength[Columns.Length];
            for (int i = 0; i < Columns.Length; i++)
                switch (Columns[i].ToLowerInvariant())
                {
                    case "auto":
                        GridLengths[i] = GridLength.Auto;
                        break;
                    case "*":
                        GridLengths[i] = GridLength.Star;
                        break;
                    case var s:
                        if (double.TryParse(s, out var n)) GridLengths[i] = n;
                        else GridLengths[i] = new GridLength();
                        break;
                }
            Return.AddRange(System.Linq.Enumerable.Select(GridLengths, x => new ColumnDefinition { Width = x }));
            return Return;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
    [AcceptEmptyServiceProvider]
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension<ImageSource>
    {
        public string Source { get; set; }

        public ImageSource ProvideValue(IServiceProvider serviceProvider = null)
        {
            if (Source == null)
            {
                return null;
            }
            string Case()
            {
                switch (Source)
                {
                    case "Forum":
                        return "forum-message-3.png";
                    case "Translate":
                        return "translator-tool-3.png";
                    case "VocabBook":
                        return "book-2.png";
                    case "Calculator":
                    case "Calculator_Free":
                        return "square-root-of-x-mathematical-signs.png";
                    case "Factorizer":
                        return "mathematical-operation.png";
                    case "Sports":
                        return "man-sprinting.png";
                    case "MusicTuner":
                        return "treble-clef-2.png";
                    case "MathSolver":
                        return "japanese-dragon.png";
                    case "Cello":
                        return "cello-icon.png";
                    case "Violin":
                        return "violin-icon.png";
                    case "Heart":
                        return "8_bit_heart_stock_by_xquatrox-d4r844m.png";
                    case "Dragon":
                        return "dragon.jpg";
                    case "Dragon_Dead":
                        return "dragon.fw.png";
                    case "File_Icon":
                        return "folded-paper_318-31112.jpg";
                    case "Facial":
                        return "boy-smiling.png";
                    case "":
                        return null;
                    default:
                        return null;
                }
            }
            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource($"{Utils.CurrentNamespace}.Images.{Case()}");

            return imageSource;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
