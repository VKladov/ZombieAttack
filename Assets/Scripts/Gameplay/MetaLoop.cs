using System.Threading;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class MetaLoop
    {
        private readonly GameLoop _gameLoop;
        private readonly Menu _menu;
        private CancellationToken _cancellation;

        public MetaLoop(GameLoop gameLoop, Menu menu)
        {
            _gameLoop = gameLoop;
            _menu = menu;
        }

        public void Start(CancellationToken cancellationToken)
        {
            _cancellation = cancellationToken;
            _menu.Open("Welcome!", new[]
            {
                new ButtonData
                {
                    Text = "Play",
                    Callback = StartPressed
                },
                new ButtonData
                {
                    Text = "Exit",
                    Callback = Exit
                }
            });
        }

        private void StartPressed()
        {
            _menu.Close();
            RunGameplay(_cancellation).Forget();
        }

        private async UniTaskVoid RunGameplay(CancellationToken cancellationToken)
        {
            await _gameLoop.Play(cancellationToken);
            ShowGameOverScreen();
        }

        private void ShowGameOverScreen()
        {
            _menu.Open("Game Over", new[]
            {
                new ButtonData
                {
                    Text = "Restart",
                    Callback = StartPressed
                },
                new ButtonData
                {
                    Text = "Exit",
                    Callback = Exit
                }
            });
        }

        private void Exit()
        {
            Application.Quit();
        }
    }
}