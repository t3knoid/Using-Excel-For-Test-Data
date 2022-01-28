# Figure 3 CribbageLib Library Structure

```C#
using System;
namespace CribbageLib
{
  public class Card
  {
    public int rank;
 // 0 = Ace, 1 = Deuce, . . , 12 = King
    public int suit;
 // 0 = Clubs, 1 = Diamonds, 2 = Hearts, 3 = Spades
    public int value;
 // Ace = 1, Deuce = 2, . . , Jack/Queen/King = 10
    public string image;
 // "Ac", "9h", etc.

    public Card(string c)
    {
      // create Card from string c
    }

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
      cards[0] = c1;
      cards[1] = c2;
      cards[2] = c3;
      cards[3] = c4;
      cards[4] = c5;
    }

    public override string ToString()
    {
      return "{ " + cards[0].ToString() + " " +
             cards[1].ToString() + " " + cards[2].ToString() + " " +
             cards[3].ToString() + " " + cards[4].ToString() + " }";
    }

    public int ValueOf15s { get { /* return point value */ } }

    public int ValueOfPairs { get { /* return point value */ } }
  } // class Hand
} // ns CribbageLib
```
