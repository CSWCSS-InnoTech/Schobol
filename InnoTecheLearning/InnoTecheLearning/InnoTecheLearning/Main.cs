using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static InnoTecheLearning.Utils;
using static InnoTecheLearning.Utils.Create;
using static InnoTecheLearning.StreamPlayer;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    public class Main : ContentPage
    {
        public enum Pages : sbyte
        {
            CloudTest = -2,
            Changelog = -1,
            Main,
            Forum,
            Translate,
            VocabBook,
            MathConverter,
            MathConverter_Duo,
            Factorizer,
            Sports,
            MusicTuner,
            MathSolver
        }
        Pages _Showing;
        Pages Showing
        {
            get { return _Showing; }
            set
            {
                switch (value)
                {
                    case Pages.CloudTest:
                        Region = "CloudTest";
                        Content = CloudTest;
                        break;
                    case Pages.Changelog:
                        Region = "Changelog";
                        Content = ChangelogView(this);
                        break;
                    case Pages.Main:
                        Region = "Main";
                        Content = MainView;
                        break;
                    case Pages.Forum:
                        Region = "Forum";
                        break;
                    case Pages.Translate:
                        Region = "Translate";
                        break;
                    case Pages.VocabBook:
                        Region = "VocabBook";
                        break;
                    case Pages.MathConverter:
                        Region = "MathConverter";
                        break;
                    case Pages.MathConverter_Duo:
                        Region = "MathConverter_Duo";
                        break;
                    case Pages.Factorizer:
                        Region = "Factorizer";
                        break;
                    case Pages.Sports:
                        Region = "Sports";
                        break;
                    case Pages.MusicTuner:
                        Region = "MusicTuner";
                        Content = MusicTuner;
                        break;
                    case Pages.MathSolver:
                        Region = "MathSolver";
                        break;
                    default:
                        Region = "App";
                        break;
                }
                _Showing = value;
            }
        }
        public Main()
        {
            MainView = _MainView;
            MusicTuner = _MusicTuner;
            BackgroundColor = Color.White;
            //Alert(this, "Main constructor");
            Showing = Pages.Main;
            Log("Main page initialized.");
        }
        protected override bool OnBackButtonPressed()
        {
            if (Showing != Pages.Main)
            {
                Showing = Pages.Main;
                return true;
            }
            else
                return base.OnBackButtonPressed();
        }

        public StackLayout MainView { get; }
        public StackLayout _MainView
        {
            get
            {
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        Title("CSWCSS eLearning App"),
                        Society,

           MainScreenRow(MainScreenItem(Image(ImageFile.Forum),delegate{
               /*Alert(this,"[2016-11-1 18:00:00] 1E03: Hi\n"+
               "[2016-11-1 18:00:09] 3F43: No one likes you loser\n[2016-11-1 18:00:16] 1E03: 😢😭😢😭😢😭😢😭😢\n"+
               "[2016-11-1 18:00:22] 2E12: Hey don't bully him!\n[2016-11-1 18:00:28] 3F43: Go kill yourself because you"+
               " are a F-ing faggot\n[2016-11-1 18:00:34] 2E12: I am going to rape you\n"+
               "[2016-11-1 18:00:55] 3F43: "+StrDup("😢😭😢😭😢😭😢😭😢",5));*/
               Showing = Pages.CloudTest;
                         }, BoldLabel("Forum (CloudTest)") ),
                         MainScreenItem(Image(ImageFile.Translate), delegate{Alert(this,
                          "I'm a translator.\nInput: eifj[vguowhfuy9q727969y\nOutput: Gud mornin turists, we spek Inglish"); },
                         BoldLabel("Translator") ),
                         MainScreenItem(Image(ImageFile.VocabBook),delegate {Alert(this,"Ida = 捱打，伸張靜儀、儆惡懲奸，\n" +
"      救死扶傷、伸張靜儀、鋤強扶弱、儆惡懲奸、修身齊家、知足常樂"); },BoldLabel("Vocab Book"))),

           MainScreenRow(MainScreenItem(Image(ImageFile.MathConverter),delegate {
                             Alert(this, "1+1=2"); },BoldLabel("Math Converter")),
                         MainScreenItem(Image(ImageFile.MathConverter_Duo),delegate {
                             Alert(this, StrDup("1+",100) + "1\n=101"); },BoldLabel("Math Converter Duo")),
                         MainScreenItem(Image(ImageFile.Factorizer),delegate {Alert(this,
                             "Factorize 3𝐗²(𝐗−1)²+2𝐗(𝐗−1)³\n = 𝐗(𝐗−1)²(5𝐗−2)"
                             ); },BoldLabel("Quadratic Factorizer"))),

           MainScreenRow(MainScreenItem(Image(ImageFile.Sports), delegate {
                             Alert(this,"🏃🏃🏃長天長跑🏃🏃🏃"); },BoldLabel("Sports")),
                         MainScreenItem(Image(ImageFile.MusicTuner), delegate {
                             Showing = Pages.MusicTuner;//Alert(this,"🎼♯♩♪♭♫♬🎜🎝♮🎵🎶\n🎹🎻🎷🎺🎸");
                         },BoldLabel("Music Tuner")),
                         MainScreenItem(Image(ImageFile.MathSolver), delegate {
                             Alert(this, "🔥🔥🔥🔥🔥🔥🐲🐉"); },BoldLabel("Maths Solver Minigame"))
                         ),

                Button((Text)"Changelog", delegate {Showing = Pages.Changelog; }),
                Utils.Create.Version
                    }
                };
            }
        }
        StreamPlayer MusicSound { get; set; }
        public StackLayout MusicTuner { get; }
        public StackLayout _MusicTuner
        {
            get
            {
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        Title("CSWCSS Music Tuner"),

                        Row(Image(ImageFile.Violin, delegate {Alert(this, "🎻♫♬♩♪♬♩♪♬"); })
                        , (Text)"Violin and Viola"),

                        Row(Button((Text)"G",  delegate {MusicSound =  Play(Sounds.Violin_G); }),
                        Button((Text)"D",  delegate {MusicSound =  Play(Sounds.Violin_D); }),
                        Button((Text)"A",  delegate {MusicSound =  Play(Sounds.Violin_A); }),
                        Button((Text)"E",  delegate {MusicSound =  Play(Sounds.Violin_E); })),

                        Row(Image(ImageFile.Cello, delegate {Alert(this, "🎻♫♬♩♪♬♩♪♬"); })
                        , (Text)"Cello and Double Bass"),

                        Row(Button((Text)"'C",  delegate {MusicSound =  Play(Sounds.Cello_C); }),
                        Button((Text)"'G",  delegate {MusicSound =  Play(Sounds.Cello_G); }),
                        Button((Text)"D",  delegate {MusicSound =  Play(Sounds.Cello_D); }),
                        Button((Text)"A",  delegate {MusicSound =  Play(Sounds.Cello_A); })),

#if __ANDROID__
                        BoldLabel("Sorry, but Android 6.0+ only!"),
#endif
                        Back(this)
                    }
                };
            }
        }
        public StackLayout CloudTest
        {
            get
            {
                var Display = new
                {
                    ID = "ID:".PadRight(8),
                    Name = "Name:".PadRight(8),
                    Class = "Class:".PadRight(8),
                    Number = "Number:".PadRight(8)
                };

                Entry ID = new Entry
                {
                    Keyboard = Keyboard.Numeric,
                    Placeholder = "Student ID (without beginning s)",
                    PlaceholderColor = Color.Gray,
                    TextColor = Color.Black,
                    Text = "18999"
                };
                Entry E = new Entry
                {
                    Keyboard = Keyboard.Text,
                    Placeholder = "Password",
                    PlaceholderColor = Color.Gray,
                    TextColor = Color.Black,
                    Text = "Y1234567"
                };
                Label L1 = BoldLabel(Display.ID);
                Label L2 = BoldLabel(Display.Name);
                Label L3 = BoldLabel(Display.Class);
                Label L4 = BoldLabel(Display.Number);

                return new StackLayout
                {
                    Children = {ID, E, Button((Text)"Test the Cloud",
                    delegate {
                    Try(delegate { var Response = Login(ToUShort(ID.Text), E.Text);
                    L1.Text = Display.ID + Response[0];    L2.Text = Display.Name + Response[1];
                    L3.Text = Display.Class + Response[2]; L4.Text = Display.Number + Response[3]; },
                    (IndexOutOfRangeException ex)=> {Alert(this, "Abnornal return value from Cloud: " +
                        string.IsNullOrWhiteSpace(ex.)) ;}); }),
                    L1, L2, L3, L4, Back(this)}
                    ,
                    VerticalOptions = LayoutOptions.Center
                };
            }
        }
#if false
        static void Hi()
        {
            Type scriptType = Type.GetTypeFromCLSID(Guid.Parse("0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC"));

            dynamic obj = Activator.CreateInstance(scriptType, false);
            obj.Language = "javascript";

            var res = obj.Eval("a=3; 2*a+32-Math.sin(6)");
        }
#endif
    }
}
