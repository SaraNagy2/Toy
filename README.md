Complete the `MeldValid` function inside `Assets/Scripts/MyController.cs` satisfying the rules outlined in the three challenges.

---

**Card representation:**
```
Integer between 0 and 51
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
- A series of cards of the same suit: ♦2 ♦3 ♦4
- Cards with the same rank (number or letter) but different suits: K♦ K♠ K♥

Ace can be at top or bottom: 
- A♥ 2♥ 3♥ 
- A♥ K♥ Q♥
```

---

Challenge 2: **Joker melding**

```
Joker can replace any card to complete a meld:
- 6♣ 7🃏 8♣ (replaced 7♣)
- 9♦ 9🃏 9♥ 9♣ (replaced 9♠)
```

---

Challenge 3: **Multi-joker melding**

```
Multiple jokers can be used to complete a meld:
- 6♣ 7🃏 8🃏 9♣ (replaced 7♣ and 8♣)
- 9♦ 9🃏 9🃏 9🃏 (replaced 9♠ and 9♥ and 9♣)
```
