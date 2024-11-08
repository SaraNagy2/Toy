Hareeg is a Middle Eastern card game where players try to get rid of their cards by melding them. The first player to empty their hand wins.

Your goal in this project is to complete the `MeldValid` function inside `Assets/Scripts/MyController.cs` that checks whether a group of selected cards constitutes a valid meld group.

---

**Card representation:**
```
Integer between 0 and 51 (a total of 52 cards: 4 suits with 13 cards each)
- 0-12:  A♣ 2♣ 3♣ … J♣ Q♣ K♣
- 13-25: A♦ 2♦ 3♦ … J♦ Q♦ K♦
- 26-38: A♠ 2♠ 3♠ … J♠ Q♠ K♠
- 39-51: A♥ 2♥ 3♥ … J♥ Q♥ K♥

Joker is represented as 200
```

---

Challenge 1: **Basic melding**

```
Must be 3-5 cards and be either:
1) Cards with the same rank (number or letter) but different suits: K♦ K♠ K♥

2) A series of cards of the same suit: ♦2 ♦3 ♦4

Note: ace can be at top or bottom: 
- A♥ 2♥ 3♥ 
- A♥ K♥ Q♥
```

---

Challenge 2: **Joker melding** (optional)

```
Joker can replace any card to complete a meld:
- 6♣ 7🃏 8♣ (replaced 7♣)
- 9♦ 9🃏 9♥ 9♣ (replaced 9♠)
```

---

Challenge 3: **Multi-joker melding** (optional)

```
Multiple jokers can be used to complete a meld:
- 6♣ 7🃏 8🃏 9♣ (replaced 7♣ and 8♣)
- 9♦ 9🃏 9🃏 9🃏 (replaced 9♠ and 9♥ and 9♣)
```
