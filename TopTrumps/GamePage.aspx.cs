﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TopTrumps


{
    public partial class GamePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)//This is needed to stop this code being called every time a button is clicked
            {
                //Need to count the cards to see if there are enough
                //Need to count the players to see if there are enough too

                string method = ListAllCategories();
                string method2 = HideButtons();
                ListBox4.Visible = false;
                ListBox5.Visible = false;
                Button2.Visible = false;
                Button3.Visible = false;
                //List<string> method3 = ListAllCards();
            }
            else
            {

            }
        }


        private string ListAllCategories()
        {
            ListBox3.Items.Clear();
            CloudTable getCategoryTable = GetTable("CategoryTable");
            TableQuery<CategoryEntity> query = new TableQuery<CategoryEntity>();//Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, categoryPartKey));
            
            foreach (CategoryEntity entity in getCategoryTable.ExecuteQuery(query))
            {
                ListBox3.Items.Add(Convert.ToString(entity.Name) + "...................................................." + "-" + (Convert.ToString(entity.PartitionKey)) + "-" + Convert.ToString(entity.RowKey));
               
            }
            

            return string.Empty;
        }

        private List<string> ListAllCards()
        {
            ListBox3.Items.Clear();
            CloudTable getCategoryTable = GetTable("CardTable");
            TableQuery<CardEntity> query = new TableQuery<CardEntity>();//Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, categoryPartKey));
            List<string> categories = new List<string> { };
            foreach (CardEntity entity in getCategoryTable.ExecuteQuery(query))
            {
                categories.Add(Convert.ToString(entity.Timestamp) + entity.ImageURI);
                ListBox3.Items.Add(Convert.ToString(entity.Timestamp)); 

            }

            categories.Sort();
            categories.Reverse();
            foreach (string t in categories)
            {
                ListBox3.Items.Add(t);
            }

            int dave = ListBox3.SelectedIndex;


            return categories;
        }

        private string ListAllPlayers()
        {
            CloudTable getCategoryTable = GetTable("PlayerTable");
            TableQuery<PlayersEntity> query = new TableQuery<PlayersEntity>();//Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, categoryPartKey));

            foreach (PlayersEntity entity in getCategoryTable.ExecuteQuery(query))
            {
                ListBox4.Items.Add(Convert.ToString(entity.PlayerName) +"...................................................." + "-" + (Convert.ToString(entity.PartitionKey)) + "-" + Convert.ToString(entity.RowKey));
                ListBox5.Items.Add(Convert.ToString(entity.PlayerName) + "...................................................." + "-" + (Convert.ToString(entity.PartitionKey)) + "-" + Convert.ToString(entity.RowKey));

            }


            return string.Empty;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Visible = true;
            Label1.Text = Convert.ToString(ListBox3.SelectedIndex);
            ListBox3.Visible = false;
            Button1.Visible = false;
            string method = ListAllPlayers();
            ListBox4.Visible = true;
            Button2.Visible = true;



        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ListBox4.Visible = false;
            Button2.Visible = false;
            ListBox5.Visible = true;
            Button3.Visible = true;




        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ListBox5.Visible = false;
            Button3.Visible = false;
            string ashTest = ListBox4.SelectedValue;
            string[] ashTest1 = ashTest.Split('-');





            //THESE ARE THE GAME METHODS
            string callMethod = EverythingVisible();
            string callMethod1 = RunGame();
            string ashleyTest = ListBox3.SelectedValue;
            string[] ashleyTest1 = ashleyTest.Split('-');
            int callMethod2 = GetNextCardRowKey(ashleyTest1[1] + ashleyTest1[2]);
            List<int> callMethod3 = GetListOfCardRowKeys(ashleyTest1[1] + ashTest1[2]);


        }

        private List<string> getPlayerDetails(string partKey, string rowKey)
        {


            CloudTable getCategoryTable = GetTable("PlayerTable");
            TableOperation retrieveData = TableOperation.Retrieve<PlayersEntity>(partKey, rowKey);
            TableResult retrieveResult = getCategoryTable.Execute(retrieveData);
            PlayersEntity playerData = (PlayersEntity)retrieveResult.Result;
            List<string> playerDetails = new List<string> { playerData.PlayerName, playerData.Wins, playerData.Losses, playerData.PartitionKey, playerData.RowKey, playerData.Games}; 


            return playerDetails;
        }

        private string updatePlayerDetails(string winner)
        {
            if (winner == "playerOne")
            {
                List<string> playerOneDetails = Session["playerOneDetails"] as List<string>;
                List<string> playerTwoDetails = Session["playerTwoDetails"] as List<string>;
                int playerOneWins = Convert.ToInt16(playerOneDetails[1]) + 1;
                int playerOneLosses = Convert.ToInt16(playerOneDetails[2]);
                int playerTwoWins = Convert.ToInt16(playerTwoDetails[1]);
                int playerTwoLosses = Convert.ToInt16(playerTwoDetails[2]) + 1;
                int playerOneGames = Convert.ToInt16(playerOneDetails[5]) + 1;
                int playerTwoGames = Convert.ToInt16(playerTwoDetails[5]) + 1;
                string playerOnePartKey = playerOneDetails[3];
                string playerOneRowKey = playerOneDetails[4];
                string playerTwoPartKey = playerTwoDetails[3];
                string playerTwoRowKey = playerTwoDetails[4];
                string method = updatePlayerTable(playerOneWins, playerOneLosses, playerOnePartKey, playerOneRowKey, playerOneGames);
                string method1 = updatePlayerTable(playerTwoWins, playerTwoLosses, playerTwoPartKey, playerTwoRowKey, playerTwoGames);

                return string.Empty;

            }


            if (winner == "playerTwo")
            {
                List<string> playerOneDetails = Session["playerOneDetails"] as List<string>;
                List<string> playerTwoDetails = Session["playerTwoDetails"] as List<string>;
                int playerOneWins = Convert.ToInt16(playerOneDetails[1]);
                int playerOneLosses = Convert.ToInt16(playerOneDetails[2]) +1;
                int playerTwoWins = Convert.ToInt16(playerTwoDetails[1]) +1;
                int playerTwoLosses = Convert.ToInt16(playerTwoDetails[2]);
                int playerOneGames = Convert.ToInt16(playerOneDetails[5]) +1;
                int playerTwoGames = Convert.ToInt16(playerTwoDetails[5]) +1;
                string playerOnePartKey = playerOneDetails[3];
                string playerOneRowKey = playerOneDetails[4];
                string playerTwoPartKey = playerTwoDetails[3];
                string playerTwoRowKey = playerTwoDetails[4];

                string method = updatePlayerTable(playerOneWins, playerOneLosses, playerOnePartKey, playerOneRowKey, playerOneGames);
                string method1 = updatePlayerTable(playerTwoWins, playerTwoLosses, playerTwoPartKey, playerTwoRowKey, playerTwoGames);

                return string.Empty;

            }

            return string.Empty;
        }

        private string updatePlayerTable(int playerWins, int playerLosses, string playerPartKey, string playerRowKey, int playerGames)
        {

            CloudTable myMessagesCloudTable = GetTable("PlayerTable");
            // Create a retrieve operation that takes a category entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<PlayersEntity>(playerPartKey, playerRowKey);
            // Execute the operation.
            TableResult retrievedResult = myMessagesCloudTable.Execute(retrieveOperation);
            // Assign the result to a CustomerEntity object.
            PlayersEntity updateEntity = (PlayersEntity)retrievedResult.Result;

            updateEntity.Wins = Convert.ToString(playerWins);
            updateEntity.Losses = Convert.ToString(playerLosses);
            updateEntity.Games = Convert.ToString(playerGames);


            // Create the Replace TableOperation.
            TableOperation updateOperation = TableOperation.Replace(updateEntity);

            // Execute the operation.
            myMessagesCloudTable.Execute(updateOperation);


            return string.Empty;
        }





        private List<int> GenerateCards()
        {
            //This effectively 'shuffles' the cards so they can be allocated between the players
            //It starts by getting a list of all the rowkeys for the selected category 
            string ashTest = ListBox3.SelectedValue;
            string[] ashTest1 = ashTest.Split('-');
            List<int> rowKeys = GetListOfCardRowKeys(ashTest1[1] + ashTest1[2]);
            //Then it counts them
            int rowKeyTotal = rowKeys.Count;
            //Then creates two new rows a cards row to store the random numbers generated
            List<int> cards = new List<int> { };
            //And gamecards to store the shuffled list
            List<int> gameCards = new List<int> { };
            Random rnd = new Random(); //Starts by generating a random number
            while (cards.Count < rowKeyTotal) //Then checks the total of the list cards is less than the total to be dealt.
            {
                int list = rnd.Next(0, rowKeyTotal);//This creates a random number between 0 and the total.
                int duplicates = 0;
                foreach (int t in cards)//This then checks to see if the number is already in the cards list.
                {
                    if (t != list)
                    {

                    }
                    else
                    {
                        duplicates = duplicates + 1;
                    }
                }

                if (duplicates == 0)//If its not a duplicate then it adds it to the list
                    //And uses the number to randomly select a rowkey and add it to the gameCard list too

                {
                    gameCards.Add(rowKeys[list]);
                    cards.Add(list);
                }
                else { }
            }
            //Once the list is complete it returns the fully shuffled pack back
            return gameCards;
        }

        private string CheckWhoHasWon(int selection)
        {
            List<int> playerOneCard = Session["playerOneCard"] as List<int>;
            int playerOneValue = playerOneCard[selection];
            List<int> playerTwoCard = Session["playerTwoCard"] as List<int>;
            int playerTwoValue = playerTwoCard[selection];
            //Runs three checks - firstly that Player One's value is higher and so Player One is the winner
            if (playerOneValue > playerTwoValue)
            {
                string callMethodP1Wins = PlayerOneWins();
                Label1.Text = "Player 1 Wins";
                if (callMethodP1Wins == "playerOne")
                {
                    return "playerOne";
                }
                else
                {
                    return string.Empty;
                }

            }

            //Then checks if Player Two#s value is higher and Player Two is the winner
            if (playerOneValue < playerTwoValue)
            {
                string callMethodP2Wins = PlayerTwoWins();
                Label1.Text = "Player 2 Wins";
                //If the return text is playerTwo then Player Two has won both the round and the game
                //So it returns either empty if just the round is won or playerTwo if the game is won too
                if (callMethodP2Wins == "playerTwo")
                {
                    return "playerTwo";
                }
                else
                {
                    return string.Empty;
                }
            }
            //Finally it checks if there has been a draw
            if (playerOneValue == playerTwoValue)
            {
                Label1.Text = "Draw";
                string callMethodDraw = Draw();
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }


        private string PlayerOneWins()
        {
            //First declare new lists playerOne and playerTwo updates and assign them to the session values for the hands
            List<int> playerOneUpdate = Session["playerOneHand"] as List<int>;
            List<int> playerTwoUpdate = Session["playerTwoHand"] as List<int>;
            //Take the first card for both playerOne and playerTwo
            int firstCard = playerOneUpdate[0];
            int secondCard = playerTwoUpdate[0];
            //Remove the first card from the list of playerOne and playerTwo
            playerOneUpdate.RemoveAt(0);
            playerTwoUpdate.RemoveAt(0);
            //Add each card to the back of playerOne's deck
            playerOneUpdate.Add(secondCard);
            playerOneUpdate.Add(firstCard);
            //Cleans the playerOneHand and playerTwoHand sessions
            Session.Remove("playerOneHand");
            Session.Remove("playerTwoHand");
            //Updates the sessions with the new decks.
            Session.Add("playerOneHand", playerOneUpdate);
            Session.Add("playerTwoHand", playerTwoUpdate);
            //Then makes it Player One's turn
            Session.Remove("whoseTurn");
            Session.Add("whoseTurn", "playerOne");
            //This checks to see if Player Two has run out of cards and therefore Player One is the game winner as well
            if (playerTwoUpdate.Count == 0)
            {
                return "playerOne";
            }
            else
            {
                return string.Empty;
            }
        }

        private string PlayerTwoWins()
        {
            //First declare new lists playerOne and playerTwo updates and assign them to the session values for the hands
            List<int> playerOneUpdate = Session["playerOneHand"] as List<int>;
            List<int> playerTwoUpdate = Session["playerTwoHand"] as List<int>;
            //Take the first card for both playerOne and playerTwo
            int firstCard = playerOneUpdate[0];
            int secondCard = playerTwoUpdate[0];
            //Remove the first card from the list of playerOne and playerTwo
            playerOneUpdate.RemoveAt(0);
            playerTwoUpdate.RemoveAt(0);
            //Add each card to the back of playerOne's deck
            playerTwoUpdate.Add(firstCard);
            playerTwoUpdate.Add(secondCard);
            //Cleans the playerOneHand and playerTwoHand sessions
            Session.Remove("playerOneHand");
            Session.Remove("playerTwoHand");
            //Updates the sessions with the new decks.
            Session.Add("playerOneHand", playerOneUpdate);
            Session.Add("playerTwoHand", playerTwoUpdate);
            //Then makes it Player Two's turn
            Session.Remove("whoseTurn");
            Session.Add("whoseTurn", "playerTwo");
            //This checks to see if Player One has run out of cards and therefore Player Two is the game winner as well
            if (playerOneUpdate.Count == 0)
            {
                return "playerTwo";
            }
            else
            {
                return string.Empty;
            }
        }

        private string Draw()
        {
            //First declare new lists playerOne and playerTwo updates and assign them to the session values for the hands
            List<int> playerOneUpdate = Session["playerOneHand"] as List<int>;
            List<int> playerTwoUpdate = Session["playerTwoHand"] as List<int>;
            //Take the first card for both playerOne and playerTwo
            int firstCard = playerOneUpdate[0];
            int secondCard = playerTwoUpdate[0];
            //Remove the first card from the list of playerOne and playerTwo
            playerOneUpdate.RemoveAt(0);
            playerTwoUpdate.RemoveAt(0);
            //Add each card to the back of playerOne's deck
            playerOneUpdate.Add(firstCard);
            playerTwoUpdate.Add(secondCard);
            //Cleans the playerOneHand and playerTwoHand sessions
            Session.Remove("playerOneHand");
            Session.Remove("playerTwoHand");
            //Updates the sessions with the new decks.
            Session.Add("playerOneHand", playerOneUpdate);
            Session.Add("playerTwoHand", playerTwoUpdate);

            return string.Empty;
        }

        private string DisableButtons()
        {
            //This simply stops the buttons being clicked again once a selection has been made
            playerOneButtonOne.Enabled = false;
            playerOneButtonTwo.Enabled = false;
            playerOneButtonThree.Enabled = false;
            playerOneButtonFour.Enabled = false;
            playerOneButtonFive.Enabled = false;
            playerTwoButtonOne.Enabled = false;
            playerTwoButtonTwo.Enabled = false;
            playerTwoButtonThree.Enabled = false;
            playerTwoButtonFour.Enabled = false;
            playerTwoButtonFive.Enabled = false;
            return string.Empty;
        }

        private string HideButtons()
        {
            //This simply stops the buttons being clicked again once a selection has been made
            playerOneButtonOne.Visible= false;
            playerOneButtonTwo.Visible = false;
            playerOneButtonThree.Visible= false;
            playerOneButtonFour.Visible= false;
            playerOneButtonFive.Visible = false;
            playerTwoButtonOne.Visible = false;
            playerTwoButtonTwo.Visible = false;
            playerTwoButtonThree.Visible = false;
            playerTwoButtonFour.Visible = false;
            playerTwoButtonFive.Visible = false;
            nextCard.Visible = false;
            playAgain.Visible = false;
            ListBox1.Visible = false;
            ListBox2.Visible = false;
            return string.Empty;
        }

        private string EnableButtons()
        {
            //This enables the buttons once again
            playerOneButtonOne.Enabled = true;
            playerOneButtonOne.BackColor = System.Drawing.Color.Empty;
            playerOneButtonOne.ForeColor = System.Drawing.Color.Empty;
            playerOneButtonTwo.Enabled = true;
            playerOneButtonTwo.BackColor = System.Drawing.Color.Empty;
            playerOneButtonTwo.ForeColor = System.Drawing.Color.Empty;
            playerOneButtonThree.Enabled = true;
            playerOneButtonThree.BackColor = System.Drawing.Color.Empty;
            playerOneButtonThree.ForeColor = System.Drawing.Color.Empty;
            playerOneButtonFour.Enabled = true;
            playerOneButtonFour.BackColor = System.Drawing.Color.Empty;
            playerOneButtonFour.ForeColor = System.Drawing.Color.Empty;
            playerOneButtonFive.Enabled = true;
            playerOneButtonFive.BackColor = System.Drawing.Color.Empty;
            playerOneButtonFive.ForeColor = System.Drawing.Color.Empty;
            playerTwoButtonOne.Enabled = true;
            playerTwoButtonOne.BackColor = System.Drawing.Color.Empty;
            playerTwoButtonOne.ForeColor = System.Drawing.Color.Empty;
            playerTwoButtonTwo.Enabled = true;
            playerTwoButtonTwo.BackColor = System.Drawing.Color.Empty;
            playerTwoButtonTwo.ForeColor = System.Drawing.Color.Empty;
            playerTwoButtonThree.Enabled = true;
            playerTwoButtonThree.BackColor = System.Drawing.Color.Empty;
            playerTwoButtonThree.ForeColor = System.Drawing.Color.Empty;
            playerTwoButtonFour.Enabled = true;
            playerTwoButtonFour.BackColor = System.Drawing.Color.Empty;
            playerTwoButtonFour.ForeColor = System.Drawing.Color.Empty;
            playerTwoButtonFive.Enabled = true;
            playerTwoButtonFive.BackColor = System.Drawing.Color.Empty;
            playerTwoButtonFive.ForeColor = System.Drawing.Color.Empty;
            return string.Empty;
        }

        private string EverythingVisible()
        {
            //This puts all images and buttons visible again
            Image1.Visible = true;
            Image2.Visible = true;
            cardNamePlayerOne.Visible = true;
            cardNamePlayerTwo.Visible = true;
            playerOneButtonOne.Visible = true;
            playerOneButtonTwo.Visible = true;
            playerOneButtonThree.Visible = true;
            playerOneButtonFour.Visible = true;
            playerOneButtonFive.Visible = true;
            playerTwoButtonOne.Visible = true;
            playerTwoButtonTwo.Visible = true;
            playerTwoButtonThree.Visible = true;
            playerTwoButtonFour.Visible = true;
            playerTwoButtonFive.Visible = true;
            ListBox1.Visible = true;
            ListBox2.Visible = true;
            return string.Empty;
        }

        private string PopulateTheScreeen()
        {
            //Starts by retrieving session data for the category, player one's hand and player two's hand
            List<string> theCategory = Session["gameCategory"] as List<string>;
            List<int> playerOneHand = Session["playerOneHand"] as List<int>;
            List<int> playerTwoHand = Session["playerTwoHand"] as List<int>;

            //Creates card entities to get azure table data for the first number in each player's hand
            CardEntity playerOneData = GetCardData(playerOneHand[0]);
            CardEntity playerTwoData = GetCardData(playerTwoHand[0]);

            //Then populates the page based on the data retrieved
            cardNamePlayerOne.Text = Convert.ToString(playerOneData.Name);
            playerOneButtonOne.Text = theCategory[1] + "  |  " + Convert.ToString(playerOneData.AttributeOne);
            playerOneButtonTwo.Text = theCategory[2] + "  |  " + Convert.ToString(playerOneData.AttributeTwo);
            playerOneButtonThree.Text = theCategory[3] + "  |  " + Convert.ToString(playerOneData.AttributeThree);
            playerOneButtonFour.Text = theCategory[4] + "  |  " + Convert.ToString(playerOneData.AttributeFour);
            playerOneButtonFive.Text = theCategory[5] + "  |  " + Convert.ToString(playerOneData.AttributeFive);
            //Next converts PartitionKey and RowKey into one variable to correspond with a named blob.
            string imageOneID = Convert.ToString(playerOneData.PartitionKey) + "-" + Convert.ToString(playerOneData.RowKey);
            //Then calls the method to obtain the blob image and put it into the image holder
            string callMethod1 = PopulateBlob1(imageOneID);
            //Finally creates a list of the five attributed ready to compare against player Two.
            List<int> playerOneCard = new List<int> { Convert.ToInt16(playerOneData.AttributeOne), Convert.ToInt16(playerOneData.AttributeTwo), Convert.ToInt16(playerOneData.AttributeThree) , Convert.ToInt16(playerOneData.AttributeFour), Convert.ToInt16(playerOneData.AttributeFive)};
            Session.Add("playerOneCard", playerOneCard);

            //Then does all the same again for player two's data.
            cardNamePlayerTwo.Text = Convert.ToString(playerTwoData.Name);
            playerTwoButtonOne.Text = theCategory[1] + "  |  " + Convert.ToString(playerTwoData.AttributeOne);
            playerTwoButtonTwo.Text = theCategory[2] + "  |  " + Convert.ToString(playerTwoData.AttributeTwo);
            playerTwoButtonThree.Text = theCategory[3] + "  |  " + Convert.ToString(playerTwoData.AttributeThree);
            playerTwoButtonFour.Text = theCategory[4] + "  |  " + Convert.ToString(playerTwoData.AttributeFour);
            playerTwoButtonFive.Text = theCategory[5] + "  |  " + Convert.ToString(playerTwoData.AttributeFive);
            string imageTwoID = Convert.ToString(playerTwoData.PartitionKey) + "-" + Convert.ToString(playerTwoData.RowKey);
            string callMethod2 = PopulateBlob2(imageTwoID);
            List<int> playerTwoCard = new List<int> { Convert.ToInt16(playerTwoData.AttributeOne), Convert.ToInt16(playerTwoData.AttributeTwo), Convert.ToInt16(playerTwoData.AttributeThree), Convert.ToInt16(playerTwoData.AttributeFour), Convert.ToInt16(playerTwoData.AttributeFive) };
            Session.Add("playerTwoCard", playerTwoCard);


            //It then makes the non-player's cards invisible
            //First checking Player One
            string turn = Session["whoseTurn"] as string;
            if (turn == "playerOne")
            {
                Image2.Visible = false;
                cardNamePlayerTwo.Visible = false;
                playerTwoButtonOne.Visible = false;
                playerTwoButtonTwo.Visible = false;
                playerTwoButtonThree.Visible = false;
                playerTwoButtonFour.Visible = false;
                playerTwoButtonFive.Visible = false;
            }
            else { }
            //Then checking if it is Player Two
            if (turn == "playerTwo")
            {
                Image1.Visible = false;
                cardNamePlayerOne.Visible = false;
                playerOneButtonOne.Visible = false;
                playerOneButtonTwo.Visible = false;
                playerOneButtonThree.Visible = false;
                playerOneButtonFour.Visible = false;
                playerOneButtonFive.Visible = false;
            }
            else { }


            //It clears then updates the output box
            ListBox1.Items.Clear();
            ListBox2.Items.Clear();
            //This puts the 'cards' in each players hand into the listboxes, so we can check it is working
            foreach (int t in playerOneHand)
            {
                ListBox1.Items.Add(Convert.ToString(t));
            }

            foreach (int t in playerTwoHand)
            {
                ListBox2.Items.Add(Convert.ToString(t));
            }
            return string.Empty;
        }

        protected void nextCard_Click(object sender, EventArgs e)
        {
            Label1.Text = "";
            nextCard.Visible = false;
            string callMethod = EnableButtons();
            string callMethod1 = PopulateTheScreeen();
        }

        protected void playerOneButtonOne_Click(object sender, EventArgs e)
        {

            //Calls the mether CheckWHoHasWon
            //This also checks if Player One is the overall winner
            //NB It is not possible to be the active player and lose in Top Trumps

            //Highlights the selected button and corresponding button for the other player
            playerOneButtonOne.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonOne.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonOne.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonOne.ForeColor = System.Drawing.Color.Black;
            //Disables the buttons so cannot be clicked twice
            string callMethod1 = DisableButtons();
            //Makes the opponent's card and values visible
            string callMethod2 = EverythingVisible();
            //Calls this method
            string theWinner = CheckWhoHasWon(0);
            //If theWinner is returned
            if (theWinner == "playerOne")
            {

                Label2.Text = "& PLAYER ONE IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);
                string callMethod3 = GameOver();
            }
            else
            {

                nextCard.Visible = true;
            }

        }

        protected void playerOneButtonTwo_Click(object sender, EventArgs e)
        {

            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonTwo.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonTwo.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonTwo.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonTwo.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(1);
            if (theWinner == "playerOne")
            {
                Label2.Text = "& PLAYER ONE IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {
                nextCard.Visible = true;
            }

        }

        protected void playerOneButtonThree_Click(object sender, EventArgs e)
        {
            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonThree.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonThree.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonThree.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonThree.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(2);
            if (theWinner == "playerOne")
            {
                Label2.Text = "& PLAYER ONE IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {
                nextCard.Visible = true;
            }

        }

        protected void playerOneButtonFour_Click(object sender, EventArgs e)
        {
            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonFour.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonFour.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonFour.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonFour.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(3);
            if (theWinner == "playerOne")
            {
                Label2.Text = "& PLAYER ONE IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {
                nextCard.Visible = true;
            }

        }

        protected void playerOneButtonFive_Click(object sender, EventArgs e)
        {
            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonFive.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonFive.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonFive.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonFive.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(4);
            if (theWinner == "playerOne")
            {
                Label2.Text = "& PLAYER ONE IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {
                nextCard.Visible = true;
            }

        }

        protected void playerTwoButtonOne_Click(object sender, EventArgs e)
        {
            //Calls the mether CheckWHoHasWon
            //This also checks if Player One is the overall winner
            //NB It is not possible to be the active player and lose in Top Trumps
            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonOne.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonOne.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonOne.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonOne.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(0);
            if (theWinner == "playerTwo")
            {
                Label2.Text = "& PLAYER TWO IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {
                nextCard.Visible = true;
            }
        }

        protected void playerTwoButtonTwo_Click(object sender, EventArgs e)
        {
            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonTwo.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonTwo.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonTwo.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonTwo.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(1);
            if (theWinner == "playerTwo")
            {
                Label2.Text = "& PLAYER TWO IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {
                nextCard.Visible = true;
            }
        }

        protected void playerTwoButtonThree_Click(object sender, EventArgs e)
        {
            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonThree.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonThree.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonThree.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonThree.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(2);
            if (theWinner == "playerTwo")
            {
                Label2.Text = "& PLAYER TWO IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {
                nextCard.Visible = true;
            }
        }

        protected void playerTwoButtonFour_Click(object sender, EventArgs e)
        {
            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonFour.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonFour.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonFour.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonFour.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(3);
            if (theWinner == "playerTwo")
            {
                Label2.Text = "& PLAYER TWO IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {

                nextCard.Visible = true;
            }
        }

        protected void playerTwoButtonFive_Click(object sender, EventArgs e)
        {
            string callMethod1 = DisableButtons();
            string callMethod2 = EverythingVisible();
            playerOneButtonFive.BackColor = System.Drawing.Color.Yellow;
            playerOneButtonFive.ForeColor = System.Drawing.Color.Black;
            playerTwoButtonFive.BackColor = System.Drawing.Color.Yellow;
            playerTwoButtonFive.ForeColor = System.Drawing.Color.Black;
            string theWinner = CheckWhoHasWon(4);
            if (theWinner == "playerTwo")
            {
                Label2.Text = "& PLAYER TWO IS VICTORIOUS!!!!";
                string callMethod4 = updatePlayerDetails(theWinner);

                string callMethod3 = GameOver();
            }
            else
            {
                nextCard.Visible = true;
            }
        }
        private string GameOver()
        {
            playAgain.Visible = true;
            return string.Empty;
        }

        private string RunGame()
        {
            //Hides next card as it only pops up when a card is selected
            nextCard.Visible = false;
            //Hides play again button as it only pops up when the game is over
            playAgain.Visible = false;
            //viewstates and sessions appear to be a way of saving variables so they can be used once an event handler is clicked!!!
            //so would be created once a login has taken place as a way of recalling the user.
            string callMethod1 = GetCategoryData();
            //creates an instance of the class DealCards
            //DealCards myCards = new DealCards();
            //calls the method GenerateCards to create the random pack of 20
            List<int> cards = GenerateCards();
            //creates two lists for each player's cards
            List<int> playerOneDeal = new List<int> { };
            List<int> playerTwoDeal = new List<int> { };
            //Then distributes the cards between Player1 and Player2
            int loopNo = cards.Count - 1;
            while (loopNo > -1)
            {
                playerOneDeal.Add(cards[loopNo]);
                loopNo = loopNo - 1;
                if (loopNo > -1)
                {
                    playerTwoDeal.Add(cards[loopNo]);
                    loopNo = loopNo - 1;
                }

            }

            //Creates session variables playerOneHand that are populated by the lists of the same name
            Session.Add("playerOneHand", playerOneDeal);
            Session.Add("playerTwoHand", playerTwoDeal);
            Session.Add("whoseTurn", "playerOne");

            //Get the Category data and populate the screen with it
            List<string> categoryData = Session["gameCategory"] as List<string>;
            gameName.Text = categoryData[0];
            //This calls the PopulateTheScreen method    
            string callMethod = PopulateTheScreeen();
            string ashTest = ListBox4.SelectedValue;
            string[] ashTest1 = ashTest.Split('-');
            string method = PopulateBlob3(Convert.ToString(ashTest1[1]) + "-" + Convert.ToString(ashTest1[2]));
            string ashTest0 = ListBox5.SelectedValue;
            string[] ashTest2 = ashTest0.Split('-');
            string method1 = PopulateBlob4(Convert.ToString(ashTest2[1]) + "-" + Convert.ToString(ashTest2[2]));
            List<string> playerOneDetails = getPlayerDetails(Convert.ToString(ashTest1[1]), Convert.ToString(ashTest1[2]));
            Session.Add("playerOneDetails", playerOneDetails);
            List<string> playerTwoDetails = getPlayerDetails(Convert.ToString(ashTest2[1]), Convert.ToString(ashTest2[2]));
            Session.Add("playerTwoDetails", playerTwoDetails);

            return string.Empty;
        }

        protected void playAgain_Click(object sender, EventArgs e)
        {

            Session.Remove("playerOneCard");
            Session.Remove("playerTwoCard");
            Session.Remove("playerOneHand");
            Session.Remove("playerTwoHand");
            Session.Remove("whoseTurn");
            Session.Remove("allCards");
            Session.Remove("gameCategory");
            Label1.Text = "";
            Label2.Text = "";
            string method = EnableButtons();
            string method1 = RunGame();
        }


        private string StorageConnectionString
        {
            get
            {
                //return "DefaultEndpointsProtocol=https;AccountName=b6039258storage;AccountKey=jOhJQMZO93hr7BuHGfnqdYQ93EauYbfyArfyJD/wKmwwyIwdCDb9XcohAn4lOz1baU0sVtEdH+J7Vg98Q/loeg==";
                return "UseDevelopmentStorage=true";
            }
        }
        private CloudTable GetTable(string tableName)
        {
            CloudStorageAccount gameStorage = CloudStorageAccount.Parse(StorageConnectionString);
            CloudTableClient gameTable = gameStorage.CreateCloudTableClient();
            CloudTable categoryTable = gameTable.GetTableReference(tableName);
            return categoryTable;
        }

        private string GetCategoryData()
        {

            CloudTable getCategoryTable = GetTable("CategoryTable");
            string ashTest = ListBox3.SelectedValue;
            string[] ashTest1 = ashTest.Split('-');
            TableOperation retrieveData = TableOperation.Retrieve<CategoryEntity>(ashTest1[1], ashTest1[2]);
            TableResult retrieveResult = getCategoryTable.Execute(retrieveData);
            CategoryEntity categoryData = (CategoryEntity)retrieveResult.Result;

            List<string> category = new List<string> { };
            category.Add(categoryData.Name);
            category.Add(categoryData.AttributeNameOne);
            category.Add(categoryData.AttributeNameTwo);
            category.Add(categoryData.AttributeNameThree);
            category.Add(categoryData.AttributeNameFour);
            category.Add(categoryData.AttributeNameFive);
            category.Add(categoryData.PartitionKey);
            category.Add(categoryData.RowKey);
            Session.Add("gameCategory", category);
            return string.Empty;
        }

        private CardEntity GetCardData(int cardNumber)
        {
            List<string> category = Session["gameCategory"] as List<string>;
            string partKey = category[6] + category[7];

            CloudTable getCardTable = GetTable("CardTable");
            TableOperation retrieveData = TableOperation.Retrieve<CardEntity>(partKey, Convert.ToString(cardNumber));
            TableResult retrieveResult = getCardTable.Execute(retrieveData);
            CardEntity cardData = (CardEntity)retrieveResult.Result;

            return cardData;
        }

        private CloudBlobContainer GetImagesBlobContainer()
        {
            // Access cloud storage account. Uses connection string obtained above.
            CloudStorageAccount myCloudStorgageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            // Create cloud table client. Provides access to Tables in your Storage Account 
            CloudBlobClient myCloudBlobClient = myCloudStorgageAccount.CreateCloudBlobClient();

            // Get Cloud Table for Message Table.
            //CloudBlob myMessagesCloudBlob = myCloudBlobClient.GetBlobReference("MessagesTable");
            CloudBlobContainer myMessagesCloudBlob = myCloudBlobClient.GetContainerReference("thegameblobs");

            // Create Messages Table if it does not already exist. 
            myMessagesCloudBlob.CreateIfNotExists();

            //The purpose of the this code is to make the images in your container publicly accessible. Without it you would not be able to see the images in your application or by using their URLs.
            myMessagesCloudBlob.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });


            // Output Messages Cloud Table object. Provides the means of accessing the Messages Table.
            return myMessagesCloudBlob;
        }

        
 
        private string PopulateBlob1(string blobReference)
        {
            CloudBlobContainer myBlobContainer = GetImagesBlobContainer();
            CloudBlockBlob myBlobIdentity = myBlobContainer.GetBlockBlobReference(blobReference);
            Image1.ImageUrl = myBlobIdentity.Uri.ToString();
            return string.Empty;
   
            
        }
        private string PopulateBlob2(string blobReference)
        {
            CloudBlobContainer myBlobContainer = GetImagesBlobContainer();
            CloudBlockBlob myBlobIdentity = myBlobContainer.GetBlockBlobReference(blobReference);
            Image2.ImageUrl = myBlobIdentity.Uri.ToString();
            return string.Empty;


        }

        private string PopulateBlob3(string blobReference)
        {
            CloudBlobContainer myBlobContainer = GetImagesBlobContainer();
            CloudBlockBlob myBlobIdentity = myBlobContainer.GetBlockBlobReference(blobReference);
            Image3.ImageUrl = myBlobIdentity.Uri.ToString();
            return string.Empty;


        }

        private string PopulateBlob4(string blobReference)
        {
            CloudBlobContainer myBlobContainer = GetImagesBlobContainer();
            CloudBlockBlob myBlobIdentity = myBlobContainer.GetBlockBlobReference(blobReference);
            Image4.ImageUrl = myBlobIdentity.Uri.ToString();
            return string.Empty;


        }

        private int GetNextCardRowKey(string categoryPartKey)
        {
            CloudTable getCardTable = GetTable("CardTable");
            TableQuery<CardEntity> query = new TableQuery<CardEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, categoryPartKey));
            List<int> outcome = new List<int> { };
            foreach (CardEntity entity in getCardTable.ExecuteQuery(query))
            {
                outcome.Add(Convert.ToInt16(entity.RowKey));

            }

            if (outcome.Count == 0)
            {
                return 0;
            }

            else
            {
                outcome.Sort();
                outcome.Reverse();
                
                int newRowKey = 1 + Convert.ToInt16(outcome[0]);
                return newRowKey;
            }
        }

        private List<int> GetListOfCardRowKeys(string categoryPartKey)
        {
            CloudTable getCardTable = GetTable("CardTable");
            TableQuery<CardEntity> query = new TableQuery<CardEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, categoryPartKey));
            List<int> outcome = new List<int> { };
            foreach (CardEntity entity in getCardTable.ExecuteQuery(query))
            {
                outcome.Add(Convert.ToInt16(entity.RowKey));

            }
            return outcome;
            

           
        }


    }
}

