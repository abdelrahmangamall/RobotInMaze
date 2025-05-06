# 🤖 Robot in Maze Path Counter

## 📖 Description
This project solves the problem of counting all valid paths for a robot navigating from the **bottom-left** to the **top-right** of an `N x M` grid. The robot can move **up**, **right**, or **diagonally up-right**, and it must avoid any obstacles (`'x'`) present in the grid.

The goal is to compute the total number of distinct paths using an efficient **dynamic programming** solution with linear complexity.

---

## 🧠 Algorithm Overview

**Inputs**
- `grid[N][M]`: A 2D character array (`'.'` = free space, `'x'` = obstacle)

**Allowed Moves**
- Move up → `(i + 1, j)`
- Move right → `(i, j - 1)`
- Move diagonally up-right → `(i + 1, j - 1)`

**Output**
- Integer: Total number of valid paths from bottom-left to top-right

**Approach**
- Use bottom-up DP: `dp[i, j]` stores the number of ways to reach cell `(i, j)`
- Initialize `dp[N-1][0] = 1`
- For each valid cell, accumulate paths from 3 directions if within bounds and not blocked

---

## ⚡ Performance

| Metric           | Value     |
|------------------|-----------|
| Time Complexity  | O(N × M)  |
| Space Complexity | O(N × M)  |

---

## 🧪 Test Results

### ✅ Trial Cases (4/4 Passed)

---

### ✅ Sample Test Cases (5/5 Passed)

| Test # | Grid Size | Result | Time (ms) |
|--------|------------|--------|------------|
| 1      | 11×10      | 1      | 0          |
| 2      | 5×3        | 0      | 0          |
| 3      | 3×4        | 13     | 0          |
| 4      | 7×3        | 2      | 0          |
| 5      | 9×6        | 412    | 0          |

---

### ✅ Complete Test Case (1/1 Passed)

| Test Case | Grid Size (N×M) | Result | Time (ms) | Timeout (ms) |
|-----------|------------------|--------|-----------|---------------|
| Case 1    | 9863 × 9352      | 24     | 1474      | 2420          |

---

## 🏁 Final Evaluation
- ✅ All test cases passed
- 🧠 Efficient and scalable (linear time)
- 💯 Final Score: **100%**

