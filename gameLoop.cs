using System;
using System.Collections.Generic;
using static consoleGame.Program;
using static consoleGame.getValue;
using static consoleGame.tools;
using static consoleGame.shuffNDeal;

namespace consoleGame
{
    class gameLoop
    {
        public static void GameLoop(int count, int playerMoney, List<string> playerHand, List<string> dealerHand, List<string> shuffled)
        {
            int dealerScore = 0;
            int playerScore = 0;
            int playerScore1 = 0;
            int playerScore2 = 0;
            int playerBet = 0;

            int splitBet1 = 0;
            int splitBet2 = 0;
            int payOut = 0;
            bool initDeal = true;
            bool blackjack = false;

            // IF player split their hand
            bool splitBool = false;

            bool playerBust = false;
            bool dealerBust = false;

            // If player stays/ stays both split hands
            bool stay = false;

            // Stay bool for split hand 1
            bool stay1 = false;
            // Stay bool for split hand 2
            bool stay2 = false;

            List<string> splitHand1 = new List<string> { " " };
            List<string> splitHand2 = new List<string> { " " };

            if ((playerScore <= 21 && dealerScore <= 21) && (stay == false))
            {
                    bool hitStay = false;

                    playerBet = ValidateEntry(count, playerMoney);
                    playerMoney -= playerBet;

                    while (!hitStay && GetIntValue(playerHand) < 22)
                    {
                        if (playerBet == 0 && playerMoney == 0)
                            continue;
                        Console.Write("....... Shuffling .......");
                        Console.Write(" Dealing .........");
                        PrintHand("Dealer's", true, dealerHand);
                        Spacer(4, "player bet: $" + playerBet);
                        PrintHand("Your", false, playerHand, playerBet, playerMoney);

                        if (GetIntValue(playerHand) == 21 && initDeal)
                        {
                            payOut = ((3 * playerBet) / 2) + playerBet;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Spacer(2, " blackjack! payout: $" + payOut);
                            AlternateColor(count);
                            Spacer(2, " press any key to continue: ");
                            Console.ReadKey();
                            blackjack = true;
                            string winString = "bet: $" + playerBet + " payout: $" + (payOut - playerBet);
                            Spacer(2, winString);
                            playerMoney += payOut;
                            payOut = 0;
                            playerBet = 0;
                            initDeal = false;
                            break;
                        }
                        // IF hand can be split
                        else if ((playerHand[0].Substring(0, 2) == playerHand[1].Substring(0, 2)) && (playerBet <= playerMoney) && initDeal)
                        {
                            char split = ValidateEntry(count, "split");
                            if (split == 'Y')
                            {
                                splitBool = true;
                                splitHand1[0] = playerHand[0];
                                splitHand2[0] = playerHand[1];
                                playerHand = new List<string> { "" };
                                splitHand1 = Hit(ref shuffled, splitHand1);
                                splitHand2 = Hit(ref shuffled, splitHand2);
                                splitBet1 = playerBet;
                                splitBet2 = playerBet;
                                playerMoney -= playerBet;
                                playerBet = 0;
                                initDeal = false;

                                // logic for splitHand1
                                while ((GetIntValue(splitHand1) < 22) && !stay1)//stay11
                                {
                                    Spacer(2, " split hand 1: ");
                                    PrintHand(" 1st split", false, splitHand1, splitBet1, playerMoney);
                                    char hit1 = ValidateEntry(count, "hit");
                                    if (hit1 == 'H')
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                        Spacer(2, "you hit");
                                        AlternateColor(count);
                                        Hit(ref shuffled, splitHand1);
                                        if (GetIntValue(splitHand1) > 21)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Spacer(2, " split hand 1 busted! ");
                                            //playerHand = splitHand2;
                                            //playerBet = splitBet2;
                                            //splitBool = false;
                                            AlternateColor(count);
                                        }
                                    }
                                    else
                                    {
                                        hitStay = true;
                                        stay1 = true;
                                    }
                                }

                                // Logic for splitHand2
                                while ((GetIntValue(splitHand2) < 22) && !stay2)//stay22
                                {
                                    Spacer(2, " split hand 2: ");
                                    PrintHand(" 2nd split", false, splitHand2, splitBet2, playerMoney);
                                    char hit2 = ValidateEntry(count, "hit");
                                    if (hit2 == 'H')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Spacer(2, "you hit");
                                        AlternateColor(count);
                                        Hit(ref shuffled, splitHand2);
                                        if (GetIntValue(splitHand2) > 21)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Spacer(2, " split hand 2 busted! ");
                                            //playerHand = splitHand1;
                                            //playerBet = splitBet1;
                                            if (playerScore1 > 21)
                                            {
                                                playerBust = true;
                                            }
                                            AlternateColor(count);
                                        }
                                    }
                                    else
                                    {
                                        stay2 = true;
                                        //stay1 = true;
                                        hitStay = true;
                                        stay = true;
                                        
                                    }
                                }
                            }
                        }
                        //  ***************************
                        // END split logic
                        //  ***************************
                        //  ***************************
                        // BEGIN single hand logic
                        //  ***************************
                        else
                        {
                            char hit = ValidateEntry(count, "hit");
                            if (hit == 'H')
                            {
                                initDeal = false;
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                Spacer(2, "you hit");
                                AlternateColor(count);
                                Hit(ref shuffled, playerHand);
                                if (GetIntValue(playerHand) > 21)
                                {
                                    playerBust = true;
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Spacer(2, "busted!");
                                    playerBet = 0;
                                    AlternateColor(count);
                                }
                            }
                            else if (hit == 'S')
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Spacer(2, " you stay ");
                                AlternateColor(count);
                                stay1 = true;
                                hitStay = true;
                                if (stay2 == true)
                                    stay = true;
                            }
                        //  ***************************
                        //END single hand logic
                        //  ***************************
                        }
                    }
                    if(!playerBust)
                    {
                        // print hands after round
                        if (splitBool)
                        {
                            playerScore1 = GetIntValue(splitHand1);
                            playerScore2 = GetIntValue(splitHand2);
                        }
                        else
                        {
                            playerScore = GetIntValue(playerHand);
                        }
                    }
                    if(!dealerBust)
                        dealerScore = GetIntValue(dealerHand);
                //}

                //  ***************************
                // BEGIN Dealer deals to themselves
                //  ***************************
                while (dealerScore < 17)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Spacer(2, "dealer hits " + (dealerHand.Count - 1) + " times.");
                    AlternateColor(count);
                    Hit(ref shuffled, dealerHand);
                    PrintHand("Dealer's", true, dealerHand);
                    dealerScore = GetIntValue(dealerHand);

                    if (dealerScore > 21)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Spacer(2, " dealer busts! ");
                        AlternateColor(count);
                        dealerBust = true;
                    }

                    Spacer(2, " press any key to continue: ");
                    Console.ReadKey();
                }
                PrintHand("Dealer's", false, dealerHand);
                //  ***************************
                // END Dealer deals to themselves
                //  ***************************


                if (splitBool)// && !playerBust
                {
                    PrintHand("1st split", false, splitHand1, splitBet1, playerMoney);
                    PrintHand("2nd split", false, splitHand2, splitBet2, playerMoney);
                }
                else// if(!playerBust)
                    PrintHand("Your", false, playerHand, playerBet, playerMoney);


                if (splitBool)
                {
                    playerScore1 = GetIntValue(splitHand1);
                    playerScore2 = GetIntValue(splitHand2);
                    // If split
                    // if dealer didn't bust
                    if(!dealerBust) {

                        if(playerScore1 <= 21 && playerScore2 <= 21) {
                            
                            if(playerScore1 > dealerScore && playerScore2 > dealerScore) {

                                payOut = 4 * (splitBet1);
                                playerMoney += payOut;
                                Spacer(2, " both hands beat the dealer! Payout: $" + payOut);
                                splitBet1 = 0;
                                splitBet2 = 0;
                                payOut = 0;

                            } else if(playerScore1 > dealerScore || playerScore2 > dealerScore) {

                                payOut = 2 * (splitBet1);
                                playerMoney += payOut;
                                Spacer(2, " one hand beat the dealer Payout: $" + payOut + "  Total bet: $" + (2 * splitBet1));
                                splitBet1 = 0;
                                splitBet2 = 0;
                                payOut = 0;

                            } else {

                                splitBet1 = 0;
                                splitBet2 = 0;
                                Spacer(2, " dealer beats both hands ");

                            }
                        } else {
                            if(playerScore1 > 21 && playerScore2 > 21) {
                                splitBet1 = 0;
                                splitBet2 = 0;
                                Spacer(2, " both hands busted ");
                            } else if(playerScore1 > 21 || playerScore2 > 21) {

                                Spacer(2, " one hand busted ");

                                if((playerScore1 <= 21 && playerScore1 > dealerScore) || (playerScore2 <= 21 && playerScore2 > dealerScore)) {

                                    payOut = 2 * (splitBet1);
                                    playerMoney += payOut;
                                    Spacer(2, " one hand beat the dealer Payout: $" + payOut + "  Total bet: $" + (2 * splitBet1));

                                } else {
                                    Spacer(2, " dealer beat your other hand ");
                                }
                            } else {
                                Spacer(2, " dealer beats both hands ");
                            }
                        }
                    // If split
                    // If dealer busted
                    } else {
                        Spacer(2, " dealer busted ");

                        if(playerScore1 <= 21 && playerScore2 <= 21) {
                                payOut = 4 * (splitBet1);
                                playerMoney += payOut;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Spacer(2, " both hands beat the dealer! Payout: $" + payOut);
                                AlternateColor(count);
                                splitBet1 = 0;
                                splitBet2 = 0;
                                payOut = 0;

                        } else if(playerScore1 <= 21 || playerScore2 <= 21) {

                                payOut = 2 * (splitBet1);
                                playerMoney += payOut;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Spacer(2, " one hand beat the dealer Payout: $" + payOut + "  Total bet: $" + (2 * splitBet1));
                                AlternateColor(count);
                                splitBet1 = 0;
                                splitBet2 = 0;
                                payOut = 0;

                        } else {

                                splitBet1 = 0;
                                splitBet2 = 0;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Spacer(2, " both hands busted ");
                                AlternateColor(count);

                        }
                    }
                }
                // If no split 
                // If dealer has busted
                else if (dealerBust)
                {
                    if (playerBust)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Spacer(2, "both dealer and player busted.");
                        AlternateColor(count);
                        playerBet = 0;
                    }
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                        Spacer(2, "dealer busts! ");
                        AlternateColor(count);

                    if (!blackjack && !playerBust)
                    {
                        playerMoney += (2 * playerBet);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Spacer(2, " you beat the dealer, Payout: $" + playerBet);
                        playerBet = 0;
                        AlternateColor(count);
                    } else if (!playerBust){
                        payOut = ((3 * playerBet) / 2) + playerBet;
                        playerMoney += payOut;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Spacer(2, " Blackjack! Payout: $" + payOut);
                        AlternateColor(count);
                        playerBet = 0;
                    }
                }
                // If no split
                // If dealer didn't bust
                else
                {
                    if (playerBust)
                    {
                        playerBet = 0;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Spacer(2, "you busted - dealer wins");
                        AlternateColor(count);
                    }
                    else
                    {
                        if (playerScore > dealerScore)
                        {
                            if (!blackjack)
                                playerMoney += (2 * playerBet);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Spacer(2, "you win! dealer score: " + dealerScore + " your score: " + playerScore);
                            Spacer(2, "Payout: $" + (2* playerBet));
                            AlternateColor(count);
                        }
                        else if (dealerScore > playerScore)
                        {
                            playerBet = 0;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Spacer(2, "you lose! dealer score: " + dealerScore + " your score: " + playerScore);
                            AlternateColor(count);
                        }
                        else if (!blackjack)
                        {
                            playerMoney += playerBet;
                            playerBet = 0;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Spacer(2, " push ");
                            AlternateColor(count);
                        }
                    }
                }

                Spacer(2, " your chips: $" + playerMoney);
                char again = ValidateEntry(count, "playAgain");
                if (again == 'N')
                    return;
                else if (again == 'Y')
                {
                    count++;
                    Spacer(2, "new game");
                    subMain(count, playerMoney);
                    return;
                }
            }
        }
    }
}