//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

namespace VikingBox
{
    public static class GameEvents
    {
        public static class Puzzle
        {
            public const string FirstEnvelopeUnlocked = "Puzzle:FirstEnvelopeUnlocked";
            public const string LastEnvelopeUnlocked = "Puzzle:LastEnvelopeUnlocked";

            // These are referenced in the editor (UnlockPuzzle component).
            public const string FirstUnlocked = "Puzzle:FirstUnlocked";
            public const string SecondUnlocked = "Puzzle:SecondUnlocked";
            public const string ThirdUnlocked = "Puzzle:ThirdUnlocked";

            public const string FirstStarted = FirstEnvelopeUnlocked;//"Puzzle:FirstStarted";
            public const string SecondStarted = FirstUnlocked;//"Puzzle:SecondStarted";
            public const string ThirdStarted = SecondUnlocked;//"Puzzle:ThirdStarted";

            public const string EndGame = "Puzzle:EndGame";
        }

        public static class Year
        {
            public const string SetInitialValue = "Year:SetInitialValue";
            public const string DigitUp = "Year:DigitUp";
            public const string DigitDown = "Year:DigitDown";
            public const string CheckAnswer = "Year:CheckAnswer";
            public const string CorrectAnswer = "Year:CorrectAnswer";
        }

        public static class Runes
        {
            public const string RuneInPlace = "Runes:RuneInPlace";
            public const string RuneRemoved = "Runes:RuneRemoved";
            public const string CheckAnswer = "Runes:CheckAnswer";
            public const string CorrectAnswer = "Runes:CorrectAnswer";
        }

        public static class Keyboard
        {
            public const string ChangeMode = "Keyboard:ChangeMode";
            public const string AfterChangeMode = "Keyboard:AfterChangeMode";
            public const string PlayNote = "Keyboard:PlayNote";
            public const string CheckAnswer = "Keyboard:CheckAnswer";
            public const string CorrectAnswer = "Keyboard:CorrectAnswer";
        }

        public static class Letter
        {
            public const string Open = "Letter:Open";
            public const string Close = "Letter:Close";
            public const string FirstLetter = "Letter:FirstLetter";
            public const string LastLetter = "Letter:LastLetter";
        }

        public static class Camera
        {
            public const string ScreenShakeStarted = "Camera:ScreenShakeStarted";
            public const string ScreenShakeFinished = "Camera:ScreenShakeFinished";
        }

        public static class Fade
        {
            public const string FadeInMenuStarted = "Fade:FadeInMenuStarted";
            public const string FadeInMenuFinished = "Fade:FadeInMenuFinished";
            public const string FadeOutMenuStarted = "Fade:FadeOutMenuStarted";
            public const string FadeOutMenuFinished = "Fade:FadeOutMenuFinished";
            public const string FadeInGameStarted = "Fade:FadeInGameStarted";
            public const string FadeInGameFinished = "Fade:FadeInGameFinished";
        }

        public static class Audio
        {
            public const string Play = "Audio:Play";
        }
    }
}