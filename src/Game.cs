using BingoBlockParty.Client.BallGame;
using BingoBlockParty.Client.BingoGame;
using BingoBlockParty.Client.InfoArea;
using BingoBlockParty.Client.LobbyArea;
using BingoBlockParty.Client.PeopleArea;
using Engine;
using Engine.Interfaces;

namespace BingoBlockParty
{
    public class Game : IGame
    { 
        public IClient Client { get;  set; }
        public AssetManager AssetManager { get; set; }
        public ILayout GameBoardLayout { get; set; }
        public ILayout InfoAreaLayout { get; set; }
        public ILayout BingoBoardLayout { get; set; }
        public ILayout PeopleAreaLayout { get; set; }

        public ILayout LobbyListLayout { get; set; }
        public IScreenManager ScreenManager { get; set; }
        public ISocket Socket { get; set; }

        public void InitScreens(IRenderer renderer, IScreenManager screenManager)
        {
            ScreenManager = screenManager;

            var gameScreen = screenManager.CreateScreen();

            GameBoardLayout = gameScreen.CreateLayout(430, 557).MakeActive().ForceTick();
            BingoBoardLayout = gameScreen.CreateLayout(332, 557);
            InfoAreaLayout = gameScreen.CreateLayout(259, 698);
            PeopleAreaLayout = gameScreen.CreateLayout(762, 202).SetScreenOrientation(ScreenOrientation.Horizontal).Offset(0, -61);

            GameBoardLayout.LeftOf(InfoAreaLayout).RightOf(BingoBoardLayout);

            PeopleAreaLayout.Below(BingoBoardLayout).Below(GameBoardLayout);

            GameBoardLayout.LayoutView = new ClientGameBoard(this, GameBoardLayout.Width, 1280, renderer, GameBoardLayout);
            BingoBoardLayout.LayoutView = new BingoLayout(this, 332, 557, renderer, BingoBoardLayout);
            InfoAreaLayout.LayoutView = new InfoAreaLayout(this, 259, 698, renderer, InfoAreaLayout);
            PeopleAreaLayout.LayoutView = new PeopleAreaLayout(this, 762, 202, renderer, PeopleAreaLayout);




            var lobbyScreen = screenManager.CreateScreen();

            LobbyListLayout = lobbyScreen.CreateLayout(766, 584). MakeActive();
            LobbyListLayout.LayoutView = new LobbyLayout(this, 766, 584, renderer, LobbyListLayout);


            screenManager.ChangeScreen(gameScreen);

        }


        public void InitSocketManager(ISocketManager socketManager)
        {
          
        }

        public class Boo
        {
            public string Foo { get; set; }
        }


        public void BeforeDraw()
        {
        }
        public void AfterDraw()
        {
        } 



        public void BeforeTick()
        {
        }
        public void AfterTick()
        {
        }

        public void LoadAssets(IRenderer renderer)
        {

            AssetManager.CreateImage("lobby.allRooms", "lobby/allRooms");
            AssetManager.CreateImage("lobby.createNewRoomText", "lobby/createNewRoomText");
            AssetManager.CreateImage("lobby.dollarSign", "lobby/dollarSign");
            AssetManager.CreateImage("lobby.emptyStar", "lobby/emptyStar");
            AssetManager.CreateImage("lobby.eyeBall", "lobby/eyeBall");
            AssetManager.CreateImage("lobby.favoritesStar", "lobby/favoritesStar");
            AssetManager.CreateImage("lobby.favoritesText", "lobby/favoritesText");
            AssetManager.CreateImage("lobby.featuredText", "lobby/featuredText");
            AssetManager.CreateImage("lobby.floor", "lobby/floor");
            AssetManager.CreateImage("lobby.joinText", "lobby/joinText");
            AssetManager.CreateImage("lobby.lobby", "lobby/lobby");
            AssetManager.CreateImage("lobby.orangeButton", "lobby/orangeButton");
            AssetManager.CreateImage("lobby.plus", "lobby/plus");
            AssetManager.CreateImage("lobby.profile", "lobby/profile");
            AssetManager.CreateImage("lobby.purpleButton", "lobby/purpleButton");
            AssetManager.CreateImage("lobby.scrollBar", "lobby/scrollBar");
            AssetManager.CreateImage("lobby.scrollButton", "lobby/scrollButton");
            AssetManager.CreateImage("lobby.selectedButton", "lobby/selectedButton");
            AssetManager.CreateImage("lobby.thinBackground", "lobby/thinBackground");
            AssetManager.CreateImage("lobby.wideBackground", "lobby/wideBackground");


            AssetManager.CreateImage("icons.lock", "icons/lock");
            AssetManager.CreateImage("icons.goldCoin", "icons/goldCoin");
            AssetManager.CreateImage("icons.100", "icons/100");
            AssetManager.CreateImage("icons.200", "icons/200");
            AssetManager.CreateImage("icons.300", "icons/300");
            AssetManager.CreateImage("icons.400", "icons/400");

            AssetManager.CreateImage("leftBoard.bingoCard", "leftBoard/bingoCard");
            AssetManager.CreateImage("leftBoard.chooseYourCards", "leftBoard/chooseYourCards");
            AssetManager.CreateImage("leftBoard.darkBoard", "leftBoard/darkBoard");
            AssetManager.CreateImage("leftBoard.greyPersonBox", "leftBoard/greyPersonBox");
            AssetManager.CreateImage("leftBoard.leftBoard", "leftBoard/leftBoard");
            AssetManager.CreateImage("leftBoard.lightBoard", "leftBoard/lightBoard");
            AssetManager.CreateImage("leftBoard.numberCallArea", "leftBoard/numberCallArea");
            AssetManager.CreateImage("leftBoard.purchaseBingCardOutline", "leftBoard/purchaseBingCardOutline");
            AssetManager.CreateImage("leftBoard.purchaseBingCardOutlineSelected", "leftBoard/purchaseBingCardOutlineSelected");
            AssetManager.CreateImage("leftBoard.purchaseBingoCard", "leftBoard/purchaseBingoCard");
            AssetManager.CreateImage("leftBoard.purchaseCard", "leftBoard/purchaseCard");
            AssetManager.CreateImage("leftBoard.purchaseConfirm", "leftBoard/purchaseConfirm");
            AssetManager.CreateImage("leftBoard.selectAvatar", "leftBoard/selectAvatar");
            AssetManager.CreateImage("leftBoard.smallGreyBox", "leftBoard/smallGreyBox");
            AssetManager.CreateImage("leftBoard.smallOrangeButton", "leftBoard/smallOrangeButton");
            AssetManager.CreateImage("leftBoard.wideGreyBox", "leftBoard/wideGreyBox");
            AssetManager.CreateImage("leftBoard.previousCalledBalls", "leftBoard/previousCalledBalls");
            AssetManager.CreateImage("leftBoard.wideOrangeButton", "leftBoard/wideOrangeButton");


            AssetManager.CreateImage("leftBoard.numbers.white.1", "leftBoard/numbers/white/1");
            AssetManager.CreateImage("leftBoard.numbers.white.2", "leftBoard/numbers/white/2");
            AssetManager.CreateImage("leftBoard.numbers.white.3", "leftBoard/numbers/white/3");
            AssetManager.CreateImage("leftBoard.numbers.white.4", "leftBoard/numbers/white/4");
            AssetManager.CreateImage("leftBoard.numbers.white.5", "leftBoard/numbers/white/5");
            AssetManager.CreateImage("leftBoard.numbers.white.6", "leftBoard/numbers/white/6");
            AssetManager.CreateImage("leftBoard.numbers.white.7", "leftBoard/numbers/white/7");
            AssetManager.CreateImage("leftBoard.numbers.white.8", "leftBoard/numbers/white/8");
            AssetManager.CreateImage("leftBoard.numbers.white.9", "leftBoard/numbers/white/9");
            AssetManager.CreateImage("leftBoard.numbers.white.10", "leftBoard/numbers/white/10");
            AssetManager.CreateImage("leftBoard.numbers.white.11", "leftBoard/numbers/white/11");
            AssetManager.CreateImage("leftBoard.numbers.white.12", "leftBoard/numbers/white/12");
            AssetManager.CreateImage("leftBoard.numbers.white.13", "leftBoard/numbers/white/13");
            AssetManager.CreateImage("leftBoard.numbers.white.14", "leftBoard/numbers/white/14");
            AssetManager.CreateImage("leftBoard.numbers.white.15", "leftBoard/numbers/white/15");
            AssetManager.CreateImage("leftBoard.numbers.white.16", "leftBoard/numbers/white/16");
            AssetManager.CreateImage("leftBoard.numbers.white.17", "leftBoard/numbers/white/17");
            AssetManager.CreateImage("leftBoard.numbers.white.18", "leftBoard/numbers/white/18");
            AssetManager.CreateImage("leftBoard.numbers.white.19", "leftBoard/numbers/white/19");
            AssetManager.CreateImage("leftBoard.numbers.white.20", "leftBoard/numbers/white/20");
            AssetManager.CreateImage("leftBoard.numbers.white.21", "leftBoard/numbers/white/21");
            AssetManager.CreateImage("leftBoard.numbers.white.22", "leftBoard/numbers/white/22");
            AssetManager.CreateImage("leftBoard.numbers.white.23", "leftBoard/numbers/white/23");
            AssetManager.CreateImage("leftBoard.numbers.white.24", "leftBoard/numbers/white/24");
            AssetManager.CreateImage("leftBoard.numbers.white.25", "leftBoard/numbers/white/25");
            AssetManager.CreateImage("leftBoard.numbers.white.26", "leftBoard/numbers/white/26");
            AssetManager.CreateImage("leftBoard.numbers.white.27", "leftBoard/numbers/white/27");
            AssetManager.CreateImage("leftBoard.numbers.white.28", "leftBoard/numbers/white/28");
            AssetManager.CreateImage("leftBoard.numbers.white.29", "leftBoard/numbers/white/29");
            AssetManager.CreateImage("leftBoard.numbers.white.30", "leftBoard/numbers/white/30");



            AssetManager.CreateImage("leftBoard.numbers.colored.1", "leftBoard/numbers/colored/1");
            AssetManager.CreateImage("leftBoard.numbers.colored.2", "leftBoard/numbers/colored/2");
            AssetManager.CreateImage("leftBoard.numbers.colored.3", "leftBoard/numbers/colored/3");
            AssetManager.CreateImage("leftBoard.numbers.colored.4", "leftBoard/numbers/colored/4");
            AssetManager.CreateImage("leftBoard.numbers.colored.5", "leftBoard/numbers/colored/5");
            AssetManager.CreateImage("leftBoard.numbers.colored.6", "leftBoard/numbers/colored/6");
            AssetManager.CreateImage("leftBoard.numbers.colored.7", "leftBoard/numbers/colored/7");
            AssetManager.CreateImage("leftBoard.numbers.colored.8", "leftBoard/numbers/colored/8");
            AssetManager.CreateImage("leftBoard.numbers.colored.9", "leftBoard/numbers/colored/9");
            AssetManager.CreateImage("leftBoard.numbers.colored.10", "leftBoard/numbers/colored/10");
            AssetManager.CreateImage("leftBoard.numbers.colored.11", "leftBoard/numbers/colored/11");
            AssetManager.CreateImage("leftBoard.numbers.colored.12", "leftBoard/numbers/colored/12");
            AssetManager.CreateImage("leftBoard.numbers.colored.13", "leftBoard/numbers/colored/13");
            AssetManager.CreateImage("leftBoard.numbers.colored.14", "leftBoard/numbers/colored/14");
            AssetManager.CreateImage("leftBoard.numbers.colored.15", "leftBoard/numbers/colored/15");
            AssetManager.CreateImage("leftBoard.numbers.colored.16", "leftBoard/numbers/colored/16");
            AssetManager.CreateImage("leftBoard.numbers.colored.17", "leftBoard/numbers/colored/17");
            AssetManager.CreateImage("leftBoard.numbers.colored.18", "leftBoard/numbers/colored/18");
            AssetManager.CreateImage("leftBoard.numbers.colored.19", "leftBoard/numbers/colored/19");
            AssetManager.CreateImage("leftBoard.numbers.colored.20", "leftBoard/numbers/colored/20");
            AssetManager.CreateImage("leftBoard.numbers.colored.21", "leftBoard/numbers/colored/21");
            AssetManager.CreateImage("leftBoard.numbers.colored.22", "leftBoard/numbers/colored/22");
            AssetManager.CreateImage("leftBoard.numbers.colored.23", "leftBoard/numbers/colored/23");
            AssetManager.CreateImage("leftBoard.numbers.colored.24", "leftBoard/numbers/colored/24");
            AssetManager.CreateImage("leftBoard.numbers.colored.25", "leftBoard/numbers/colored/25");
            AssetManager.CreateImage("leftBoard.numbers.colored.26", "leftBoard/numbers/colored/26");
            AssetManager.CreateImage("leftBoard.numbers.colored.27", "leftBoard/numbers/colored/27");
            AssetManager.CreateImage("leftBoard.numbers.colored.28", "leftBoard/numbers/colored/28");
            AssetManager.CreateImage("leftBoard.numbers.colored.29", "leftBoard/numbers/colored/29");
            AssetManager.CreateImage("leftBoard.numbers.colored.30", "leftBoard/numbers/colored/30");


            AssetManager.CreateImage("board", "gameBoards/board1");

            AssetManager.CreateImage("walkway.red", "walkway/walkway-red");
            AssetManager.CreateImage("walkway.barrier", "walkway/walkwayBarrier");

            renderer.CreateImage("peg.white", "pegs/white_peg", new Point(13, 9));
            AssetManager.CreateImage("peg.hit.white", "pegs/white_peg_lit_overlay");

            renderer.CreateImage("peg.blue", "pegs/blue_peg", new Point(13, 9));
            AssetManager.CreateImage("peg.hit.blue", "pegs/blue_peg_lit_overlay");

            renderer.CreateImage("peg.green", "pegs/green_peg", new Point(13, 9));
            AssetManager.CreateImage("peg.hit.green", "pegs/green_peg_lit_overlay");

            renderer.CreateImage("peg.purple", "pegs/purple_peg", new Point(13, 9));
            AssetManager.CreateImage("peg.hit.purple", "pegs/purple_peg_lit_overlay");

            renderer.CreateImage("peg.red", "pegs/red_peg", new Point(13, 9));
            AssetManager.CreateImage("peg.hit.red", "pegs/red_peg_lit_overlay");

            renderer.CreateImage("peg.yellow", "pegs/yellow_peg", new Point(16, 8));
            AssetManager.CreateImage("peg.hit.yellow", "pegs/yellow_peg_lit_overlay");


            AssetManager.CreateImage("cannon", "cannons/shooter");
            AssetManager.CreateImage("cannonBall", "cannonBalls/ball_inner");
            AssetManager.CreateImage("cannonBallShine", "cannonBalls/ball_outer");

            AssetManager.CreateImage("chuteBuckets", "chutes/buckets");
            AssetManager.CreateImage("chuteBumpers", "chutes/bumpers");

            AssetManager.CreateImage("chuteGreenBucket", "chutes/green_bucket");
            AssetManager.CreateImage("chutePurpleBucket", "chutes/purple_bucket");
            AssetManager.CreateImage("chuteRedBucket", "chutes/red_bucket");
            AssetManager.CreateImage("chuteYellowBucket", "chutes/yellow_bucket");
            AssetManager.CreateImage("chuteBlueBucket", "chutes/blue_bucket");

            AssetManager.CreateImage("chuteGreenBucketLit", "chutes/green_bucket_lit");
            AssetManager.CreateImage("chutePurpleBucketLit", "chutes/purple_bucket_lit");
            AssetManager.CreateImage("chuteRedBucketLit", "chutes/red_bucket_lit");
            AssetManager.CreateImage("chuteYellowBucketLit", "chutes/yellow_bucket_lit");
            AssetManager.CreateImage("chuteBlueBucketLit", "chutes/blue_bucket_lit");

            AssetManager.CreateImage("jackpotOverlay", "overlays/jackpot_shooter_overlay");
            AssetManager.CreateImage("coinBoxOverlay", "overlays/coin_box");
            AssetManager.CreateImage("silverCoinBoxOverlay", "overlays/silver_coin_box");
            AssetManager.CreateImage("pullBoxOverlay", "overlays/pulls_button");

            AssetManager.CreateImage("female.blonde.front", "people/Female1_FrontDesign");
            AssetManager.CreateImage("male.hat.front", "people/male2_FrontDesign");


        }

        public void LoadFonts(IRenderer renderer)
        {
            renderer.CreateFont("lobby.font", "spriteFont1");
            renderer.CreateFont("Arial-18", "Arial-18");
        }

      
    }

}
