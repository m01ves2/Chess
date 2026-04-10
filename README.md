# Chess Project
 
# Project Overview
This project is a custom implementation of a chess game written in C#, featuring a fully playable chess engine, AI opponent, and a custom console-based UI framework.
The goal of the project was not only to implement chess rules, but also to explore architecture, game state management, and basic game AI techniques such as Minimax search.

# Features
## Core Chess Logic
Full chess rules implementation:
- Legal move generation
- Check / checkmate detection
- Castling (king-side and queen-side)
- En passant
- Pawn promotion
- Strongly-typed domain model based on inheritance


## Game Engine
Engine designed as a service layer
Separation between:
-Domain (GamePosition, Board, Pieces)
-Engine (rules, move generation, validation)
-Application layer (game flow, AI, UI)
-Stateless move execution via engine methods

## State Management
-Snapshot-based state system
-Undo/Redo support via stack of game snapshots
-Deterministic reconstruction of game state

## AI System
- Minimax algorithm
- Alpha-Beta pruning optimization
- Move ordering heuristics (MVV-LVA, positional heuristics)
- Transposition table (position caching)
- Quiescence search for tactical stability
- Cycle detection (repeated positions handling)
- Configurable AI difficulty (search depth)

## Evaluation System
- Material-based evaluation
- Additional heuristics:
- Center control
- Piece activity
- Pawn movement incentives
- Heuristic move scoring for better search efficiency

## UI Framework (Custom CLI Engine)
- Custom console rendering system
- Screen / panel architecture
- Double buffering rendering pipeline
- Shared screen buffer for UI composition
- Lightweight CLI framework for game visualization

## Players System
-Human player implementation
-AI player implementation
-AI vs AI simulation mode
-Configurable difficulty levels


# Architecture Highlights

This project explores a layered architecture approach:

UI Layer (CLI)
    ↓
Application Layer
    ↓
Game Controller / Flow
    ↓
Engine (rules & validation)
    ↓
Domain Model (Board, Pieces, Moves)

Key design principles:
- Separation of concerns
- Immutable game state snapshots
- Explicit move execution pipeline
- Minimal coupling between UI, engine, and AI

# Performance Notes
- Minimax depth: typically 1-3
- Performance bottleneck: move generation and board cloning
- Optimization techniques significantly reduce search space:
    + Alpha-beta pruning
    + Move ordering
    + Transposition table caching

# Known Limitations
- Depth-limited search leads to horizon effect
- Simplified evaluation function
- No opening book
- No endgame tablebase support
- Snapshot cloning introduces performance overhead

# Future Improvements
- Graphic UI instead of CLI
- Replace snapshot cloning with Do/Undo move system
- Iterative deepening search
- Zobrist hashing for faster transposition keys
- Improved evaluation (piece-square tables, king safety)
- Opening book integration
- Parallel search (multi-threaded minimax)

# What I Learned
- Game tree search algorithms (Minimax, Alpha-Beta pruning)
- Trade-offs between correctness and performance
- Importance of evaluation design in AI behavior
- Complexity of state management in board games
- Architectural layering in medium-sized systems
- Debugging emergent behavior in AI systems

# Why This Project Matters

This project goes beyond a simple game implementation.

It demonstrates:
ability to design and structure a medium-sized system
understanding of algorithms and optimization techniques
experience with state-heavy logic and simulations
ability to debug complex, non-deterministic behavior