using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


/*
****current issues****
****To-do****
-Create a score board based on the number of players in the game
 */
namespace farklev_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //returns the score after calculating it
        private int calScore(int[] numsRolled)
        {
            int score = 0;
            int setsOf3 = 0;
            int pairsOf2 = 0;
            Boolean kindOf6 = false;
            Boolean kindOf5 = false;
            Boolean kindOf4 = false;

            //checks all of the numbers rolled and amount of them they were rolled
            for (int i = 0; i < 6; i++)
            {
                if (numsRolled[i] == 6)
                {
                    numsRolled[i] -= 6;
                    kindOf6 = true;
                }
                else if (numsRolled[i] == 5)
                {
                    numsRolled[i] -= 5;

                    kindOf5 = true;
                }
                else if (numsRolled[i] == 3)
                {
                    setsOf3 += 3;
                }
                else if (numsRolled[i] == 2)
                {
                    pairsOf2 += 2;
                }
                else if (numsRolled[i] == 4)
                {
                    numsRolled[i] -= 4;

                    kindOf4 = true;
                }
            }

            //adds to the score depending on all the dice (needs to be seperate for each roll)
            if (kindOf6)
            {
                score += 3000;
            }
            else if (kindOf5)
            {
                score += 2000;
            }
            else if (setsOf3 == 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (numsRolled[i] == 3)
                        numsRolled[i] -= 3;
                }

                score += 2500;
            }
            else if (pairsOf2 == 6 || (kindOf4 && pairsOf2 == 2))
            {
                for (int i = 0; i < 6; i++)
                {
                    if (numsRolled[i] == 2)
                        numsRolled[i] -= 2;
                }

                score += 1500;
            }
            else if (numsRolled[0] == 1 && numsRolled[1] == 1 && numsRolled[2] == 1 && numsRolled[3] == 1 && numsRolled[4] == 1 && numsRolled[5] == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    numsRolled[i]--;
                }
                score += 1500;
            }
            else if (kindOf4)
            {
                score += 1000;
            }
            else if (numsRolled[5] == 3)
            {
                numsRolled[5] -= 3;
                score += 600;
            }
            else if (numsRolled[4] == 3)
            {
                numsRolled[4] -= 3;
                score += 500;
            }
            else if (numsRolled[3] == 3)
            {
                numsRolled[3] -= 3;
                score += 400;
            }
            else if (numsRolled[2] == 3)
            {
                numsRolled[2] -= 3;
                score += 300;
            }
            else if (numsRolled[0] == 3)
            {
                numsRolled[0] -= 3;
                score += 300;
            }
            else if (numsRolled[1] == 3)
            {
                numsRolled[1] -= 3;
                score += 200;
            }

            //Checks if thre are any left over 1's and 5's and then adds those to the score
            if (numsRolled[0] > 0 || numsRolled[4] > 0)
            {
                score += numsRolled[0] * 100;
                score += numsRolled[4] * 50;

                numsRolled[0] = 0;
                numsRolled[4] = 0;
            }

            return score;
        }

        //detects if there is a farkle and returns if there is or is not a farkle
        private Boolean farkle(int[] numsRolled)
        {
            Boolean detector = false;
            int tempScore = calScore(numsRolled);  //uses the function to see if there is a score from the resulting roll. If there is no score then there is a farkle

           //MessageBox.Show(tempScore.ToString());  //uses for testing purposes

            if (tempScore == 0)  //If there is no score then there is a farkle
            {
                detector = true;
            }

            return detector;
        }

        //checks for hot dice depending on if the player kept all the dice
        private Boolean hotDice(Boolean[] keep)
        {
            Boolean detect = false;
            int count = 0;

            for (count = 0; count < 6; count++)
            {
                if (!keep[count])
                    break;
            }

            if (count == 6)
            {
                detect = true;
            }

            return detect;
        }

        //tracks when the threshold for winning the game is met
        private void reachThreshold(int totalScore, int player)
        {
            player++;
            if (totalScore >= 10000 && playerWinner == -1)  //Set to 1000 for testing purposes (it's supposed to be 10000)
            {
                MessageBox.Show("Player " + player + " just reached the threshold of 10000 points.\nEvery other player has one more chance to beat player " + player + "'s score of " + totalScore);
                playerWinner = --player;
            }
        }

        //Finds the winner of the game and closes the program
        private void winner(int[] totalScores)
        {
            int numPeopleTie = 1;
            int tieNum = -1;
            int[] tieArr = { 0 };
            Boolean tieBool = false;
            Array.Resize(ref tieArr, players);

            int max = totalScores[0];
            int maxIndex = 0;
            for (int i = 0; i < players - 1; i++)
            {
                i++;
                if (max < totalScores[i])
                {
                    playerWinner = i;
                    max = totalScores[i];
                    maxIndex = i;
                } else if (max == totalScores[i])
                {
                    tieNum = max;
                    tieArr[maxIndex] = 1;
                    tieArr[i] = 1;
                    tieBool = true;
                    numPeopleTie++;
                }
                i--;
            }
            
            if (tieBool && max ==  tieNum)
            {
                string tieErs = "There was a tie between players ";
                for (int i = 0; i < tieArr.Length; i++)
                {
                    if (tieArr[i] == 1 && numPeopleTie == 1)
                    {
                        i++;
                        tieErs += "and " + (i).ToString();
                        i--;
                    } else if (tieArr[i] == 1 && numPeopleTie == 2)
                    {
                        i++;
                        tieErs += (i).ToString() + " ";
                        i--;
                        numPeopleTie--;
                    } else if (tieArr[i] == 1)
                    {
                        i++;
                        tieErs += (i).ToString() + ", ";
                        i--;
                        numPeopleTie--;
                    }
                }
                MessageBox.Show(tieErs);
            } 
            else
            {
                MessageBox.Show("The winner is player: " + ++playerWinner + " with a score of: " + totalScores[--playerWinner]);
            }
            this.Close();
        }

        //gets how many times each number was rolled
        private void getNumsRolledFalse(int[] dice, int[] numsRolled, Boolean[] keep)
        {
            for (int i = 0; i < 6; i++)
            {
                if (dice[i] == 1 && !keep[i])
                {
                    numsRolled[0]++;
                }
                else if (dice[i] == 2 && !keep[i])
                {
                    numsRolled[1]++;
                }
                else if (dice[i] == 3 && !keep[i])
                {
                    numsRolled[2]++;
                }
                else if (dice[i] == 4 && !keep[i])
                {
                    numsRolled[3]++;
                }
                else if (dice[i] == 5 && !keep[i])
                {
                    numsRolled[4]++;
                }
                else if (dice[i] == 6 && !keep[i])
                {
                    numsRolled[5]++;
                }
            }
        }

        //displays the rolled number for a specific die in the form
        private void showDice(int[] dice, PictureBox[] diceImages)
        {
            for (int i = 0; i < 6; i++)
            {
                if (dice[i] == 1 && !keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die1_white;
                }
                else if (dice[i] == 2 && !keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die2_white;
                }
                else if (dice[i] == 3 && !keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die3_white;
                }
                else if (dice[i] == 4 && !keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die4_white;
                }
                else if (dice[i] == 5 && !keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die5_white;
                }
                else if (dice[i] == 6 && !keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die6_white;
                }
                else if (dice[i] == 1 && keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die1_grey;
                }
                else if (dice[i] == 2 && keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die2_grey;
                }
                else if (dice[i] == 3 && keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die3_grey;
                }
                else if (dice[i] == 4 && keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die4_grey;
                }
                else if (dice[i] == 5 && keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die5_grey;
                }
                else if (dice[i] == 6 && keep[i])
                {
                    diceImages[i].Image = Properties.Resources.Die6_grey;
                }
            }
        }

        //Used for updating one dice. Saves on resources by not having to "redraw" all the dice
        private void updateDie(int die, int[] dice, PictureBox[] diceImages)
        {
            if (dice[die] == 1 && !keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die1_white;
            }
            else if (dice[die] == 2 && !keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die2_white;
            }
            else if (dice[die] == 3 && !keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die3_white;
            }
            else if (dice[die] == 4 && !keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die4_white;
            }
            else if (dice[die] == 5 && !keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die5_white;
            }
            else if (dice[die] == 6 && !keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die6_white;
            }
            else if (dice[die] == 1 && keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die1_grey;
            }
            else if (dice[die] == 2 && keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die2_grey;
            }
            else if (dice[die] == 3 && keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die3_grey;
            }
            else if (dice[die] == 4 && keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die4_grey;
            }
            else if (dice[die] == 5 && keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die5_grey;
            }
            else if (dice[die] == 6 && keep[die])
            {
                diceImages[die].Image = Properties.Resources.Die6_grey;
            }
        }

        //resets the array of dice that cant be selected
        private void resetDiceCantSelect()
        {
            for (int i = 0; i < 6; i++)
            {
                diceCantSelect[i] = 0;
            }
        }

        //resets the keep array to false
        private void resetKeep()
        {
            for (int i = 0; i < 6; i++)
            {
                keep[i] = false;
            }
        }

        //sets all the values in the keep array to false
        private void resetNumsRolledTracker()
        {
            for (int i = 0; i < 6; i++)
            {
                numsRolledKeepFalse[i] = 0;
            }
        }

        //makes all the values in the numsKeep array set to 0, reseting them
        private void resetNumsKeep()
        {
            for (int i = 0; i < 6; i++)
            {
                numsKeep[i] = 0;
            }
        }

        //Keeps track of the die that was clicked and what roll number it was
        private void dieClicked(int num)
        {
            PictureBox[] diceImages = { roll1, roll2, roll3, roll4, roll5, roll6 };
            num--;  //for getting the correct index in the arrays

            if (!keep[num] && diceCantSelect[num] == 0)  //checks to see if the user clicked the image and sets the keep value appropriately
            {
                keep[num] = true;
                numsKeep[dice[num] - 1]++;
            }
            else if (diceCantSelect[num] == 1)
            {
                MessageBox.Show("You cannot keep that die");
            }
            else
            {
                keep[num] = false;
                numsKeep[dice[num] - 1]--;
            }
            updateDie(num, dice, diceImages);
        }

        //Ends the current players turn and resets all the global variables, used when there is a score
        private void endTurn(int score, int[] dice, int[] numsKeep, Boolean[] keep)
        {
            PictureBox[] diceImages = { roll1, roll2, roll3, roll4, roll5, roll6 };

            score += calScore(numsKeep);
            resetNumsKeep();

            //All these lines of code resets the global variables and the button text
            totalScores[player] += score;
            MessageBox.Show("Total score for player " + ++player + " is: " + totalScores[--player].ToString());
            this.score = 0;
            rollButton.Text = "Roll";
            endTurnButton.Visible = false;
            label1.Visible = false;
            resetNumsRolledTracker();
            resetDiceCantSelect();
            for (int i = 0; i < 6; i++)
            {
                diceImages[i].Image = null;
                dice[i] = 0;
                keep[i] = false;
            }

            //Checks to see if only one player is playing
            if (players != 1)
            {
                reachThreshold(totalScores[player], player);  //checks to see if any player has crossed the threshold

                if (player == players - 1)
                    player = 0;
                else
                    player++;

                //Lets the rest of the players take their turns. When everyone has gone one more time, the winner is found
                if (playerWinner > -1 && endCounter != 0)
                    endCounter--;
                else if (endCounter == 0)
                    winner(totalScores);
            } else
            {
                if (totalScores[0] >= 10000)
                {
                    MessageBox.Show("You won against yourself!!!");
                    this.Close();
                }
            }
        }

        //Ends the current players turn and resets all the global variables, used when the score is 0
        private void endTurn()
        {
            PictureBox[] diceImages = { roll1, roll2, roll3, roll4, roll5, roll6 };

            score = 0;
            rollButton.Text = "Roll";
            endTurnButton.Visible = false;
            label1.Visible = false;
            resetNumsRolledTracker();
            resetDiceCantSelect();
            for (int i = 0; i < 6; i++)
            {
                diceImages[i].Image = null;
                dice[i] = 0;
                keep[i] = false;
            }

            if (players != 1)
            {
                if (player == players - 1)
                    player = 0;
                else
                    player++;

                if (playerWinner > -1 && endCounter != 0)
                    endCounter--;
                else if (endCounter == 0)
                    winner(totalScores);
            }
        }

        int[] totalScores = { 0 };
        int score = 0;
        int players = 0;
        int player = 0;
        int playerWinner = -1;
        int endCounter = -1;
        Boolean keptAtleastOne = false;
        Boolean[] keep = { false, false, false, false, false, false };
        int[] dice = { 0, 0, 0, 0, 0, 0 };
        int[] diceCantSelect = { 0, 0, 0, 0, 0, 0 };
        int[] numsRolledKeepFalse = { 0, 0, 0, 0, 0, 0 };
        int[] numsKeep = { 0, 0, 0, 0, 0, 0 };

        //Rolls the dice and houses the main parts of the program
        private void rollButton_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            PictureBox[] diceImages = { roll1, roll2, roll3, roll4, roll5, roll6 };

            for (int i = 0; i < 6; i++)
            {
                if (keep[i])
                    keptAtleastOne = true;
            }

            //The first roll of the player
            if (rollButton.Text == "Roll")
            {
                for (int i = 0; i < 6; i++)
                {
                    dice[i] = rnd.Next(1, 7);  //sets the values of dice to what is rolled
                }

                showDice(dice, diceImages);  //displays the dice rolls on the form

                //changes the form to show the rest of the players turn
                rollButton.Text = "Roll again?";
                endTurnButton.Visible = true;
                label1.Visible = true;

                getNumsRolledFalse(dice, numsRolledKeepFalse, keep);
                if (farkle(numsRolledKeepFalse))
                {
                    MessageBox.Show("You Farkled");
                    endTurn();
                }
                else  //uses the numsRolledKeepFalse array to tell if there are any left over dice that did not contribute to the score and makes them unselectable. Uses the farkle funciton call to do this, the one in the if statement
                {     //This is a clumsy solution and should have it's own funciton for it. It should also not depend on a funciotn that is supposed to calcualte a score 
                    for (int i = 0; i < 6; i++)
                    {
                        if (!keep[i] && numsRolledKeepFalse[dice[i] - 1] > 0)  //numsRolledKeepFalse only contains the numbers that did not contribute to a score since calScore was called in the farkle funciton
                        {
                            diceCantSelect[i] = 1;
                        }
                    }
                }
                resetNumsRolledTracker();
            }
            else if (rollButton.Text == "Roll again?" && keptAtleastOne)  //The subsequent rolls after the first roll
            {
                score += calScore(numsKeep);
                resetNumsKeep();  //resets the numbers that are rolled since their purpose was used (to calculate the score)
                resetDiceCantSelect();
                if (hotDice(keep))
                {
                    resetKeep();
                }

                for (int i = 0; i < 6; i++)  //re-rolls the dice that the user did not keep
                {
                    if (!keep[i])
                    {
                        dice[i] = rnd.Next(1, 7);
                    }
                }

                showDice(dice, diceImages);

                //These lines of code detect a farcle. The first line get the current roll of the dice. Then the if statement checks to see if the player farkled. Depending on if they did, a specific operation happens
                //The last line empties out the numsRolledKeepFasle array because it's use has been fulfilled
                getNumsRolledFalse(dice, numsRolledKeepFalse, keep);
                if (farkle(numsRolledKeepFalse))
                {
                    MessageBox.Show("You Farkled");
                    score = 0;
                    endTurn();
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (!keep[i] && numsRolledKeepFalse[dice[i] - 1] > 0)
                        {
                            diceCantSelect[i] = 1;
                        }
                    }
                }
                resetNumsRolledTracker();
            }
            else if (!keptAtleastOne)
            {
                MessageBox.Show("You must keep at least one to roll again");
            }
        }

        //Ends the current players turn and resets all the global variables
        private void endTurnButton_Click(object sender, EventArgs e)
        {
            endTurn(score, dice, numsKeep, keep);
        }

        //Takes the input form the user and sets the form for the game
        private void enterButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(peoplePlayingTextBox.Text, out players) || players <= 0)
            {
                MessageBox.Show("There was an error with your input. Try again.");
            }
            else {
                label2.Visible = false;
                peoplePlayingTextBox.Visible = false;
                enterButton.Visible = false;
                rollButton.Visible = true;

                endCounter = players - 1;
                Array.Resize(ref totalScores, players);
            }
        }

        //These clicks track what the user wants to keep/not keep and stores that number in the numbers to keep array
        private void roll1_Click(object sender, EventArgs e)
        {
            if (roll1.Image != null)
            {
                dieClicked(1);
            }
        }

        private void roll2_Click(object sender, EventArgs e)
        {
            if (roll1.Image != null)
            {
                dieClicked(2);
            }
        }

        private void roll3_Click(object sender, EventArgs e)
        {
            if (roll1.Image != null)
            {
                dieClicked(3);
            }
        }

        private void roll4_Click(object sender, EventArgs e)
        {
            if (roll1.Image != null)
            {
                dieClicked(4);
            }
        }

        private void roll5_Click(object sender, EventArgs e)
        {
            if (roll1.Image != null)
            {
                dieClicked(5);
            }
        }

        private void roll6_Click(object sender, EventArgs e)
        {
            if (roll1.Image != null)
            {
                dieClicked(6);
            }
        }
    }
}
