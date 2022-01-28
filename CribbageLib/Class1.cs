using System;
//using System.Collections.Generic;
//using System.Text;

namespace CribbageLib
{
  public class Card
  {
    public int rank;  // 0 = Ace, 1 = Deuce, . . , 12 = King
    public int suit;  // 0 = Clubs, 1 = Diamonds, 2 = Hearts, 3 = Spades
    public int valu;  // Ace = 1, Deuce = 2, . . , Jack/Queen/King = 10
    public string image;  // "Ac", "9h", etc.

    public Card(string c)
    {
      char r = c[0]; // rank
      char s = c[1]; // suit

      if (r == 'A') rank = 0; if (r == '2') rank = 1; if (r == '3') rank = 2;
      if (r == '4') rank = 3; if (r == '5') rank = 4; if (r == '6') rank = 5;
      if (r == '7') rank = 6; if (r == '8') rank = 7; if (r == '9') rank = 8;
      if (r == 'T') rank = 9; if (r == 'J') rank = 10; if (r == 'Q') rank = 11;
      if (r == 'K') rank = 12; 

      if (s == 'c') suit = 0; // clubs
      if (s == 'd') suit = 1; // diamonds
      if (s == 'h') suit = 2;
      if (s == 's') suit = 3;

      if (r == 'A') valu = 1; if (r == '2') valu = 2; if (r == '3') valu = 3;
      if (r == '4') valu = 4; if (r == '5') valu = 5; if (r == '6') valu = 6;
      if (r == '7') valu = 7; if (r == '8') valu = 8; if (r == '9') valu = 9;
      if (r == 'T') valu = 10; if (r == 'J') valu = 10; if (r == 'Q') valu = 10;
      if (r == 'K') valu = 10;

      image = c;
    } // Card() constructor

    public override string ToString()
    {
      return image;
    }
  } // class Card

  public class Hand
  {
    Card[] cards;

    public Hand(Card c1, Card c2, Card c3, Card c4, Card c5)
    {
      cards = new Card[5];
      cards[0] = c1; cards[1] = c2; cards[2] = c3;
      cards[3] = c4; cards[4] = c5;
    }

    public override string ToString()
    {
      return "{ " + cards[0].ToString() + " " + cards[1].ToString() + " " + cards[2].ToString() + " " + cards[3].ToString() + " " + cards[4].ToString() + " }";
    }

    public int ValueOf15s
    {

      get
      {
        int ans = 0;

        if (this.cards[0].valu + this.cards[1].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[2].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[3].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[1].valu + this.cards[2].valu == 15) ans += 2;
        if (this.cards[1].valu + this.cards[3].valu == 15) ans += 2;
        if (this.cards[1].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[2].valu + this.cards[3].valu == 15) ans += 2;
        if (this.cards[2].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[3].valu + this.cards[4].valu == 15) ans += 2;

        if (this.cards[0].valu + this.cards[1].valu + this.cards[2].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[1].valu + this.cards[3].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[1].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[2].valu + this.cards[3].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[2].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[3].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[1].valu + this.cards[2].valu + this.cards[3].valu == 15) ans += 2;
        if (this.cards[1].valu + this.cards[2].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[1].valu + this.cards[3].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[2].valu + this.cards[3].valu + this.cards[4].valu == 15) ans += 2;

        if (this.cards[0].valu + this.cards[1].valu + this.cards[2].valu + this.cards[3].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[1].valu + this.cards[2].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[1].valu + this.cards[3].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[0].valu + this.cards[2].valu + this.cards[3].valu + this.cards[4].valu == 15) ans += 2;
        if (this.cards[1].valu + this.cards[2].valu + this.cards[3].valu + this.cards[4].valu == 15) ans += 2;

        if (this.cards[0].valu + this.cards[1].valu + this.cards[2].valu + this.cards[3].valu + this.cards[4].valu== 15) ans += 2;

        return ans;
      }
    } // property ValueOf15s

    public int ValueOfPairs
    {
      get
      {
        int ans = 0;

        if (this.cards[0].rank == this.cards[1].rank) ans += 2;
        if (this.cards[0].rank == this.cards[2].rank) ans += 2;
        if (this.cards[0].rank == this.cards[3].rank) ans += 2;
        if (this.cards[0].rank == this.cards[4].rank) ans += 2;
        if (this.cards[1].rank == this.cards[2].rank) ans += 2;
        if (this.cards[1].rank == this.cards[3].rank) ans += 2;
        if (this.cards[1].rank == this.cards[4].rank) ans += 2;
        if (this.cards[2].rank == this.cards[3].rank) ans += 2;
        if (this.cards[2].rank == this.cards[4].rank) ans += 2;
        if (this.cards[3].rank == this.cards[4].rank) ans += 2;

        return ans;
      }
    } // property ValueOfPairs





  } // class Hand

} // ns CribbageLib
