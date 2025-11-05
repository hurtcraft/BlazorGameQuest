namespace BlazorGameQuestClassLib
{
    public interface IAnimable
    {
        /// <summary>
        /// Démarre une animation spécifique (par ex : "walk", "attack", "idle", etc.)
        /// </summary>
        void PlayAnimation(string animationName);

        /// <summary>
        /// Met à jour l’état de l’animation (avancement du frame, etc.)
        /// </summary>
        void UpdateAnimation(float deltaTime);

        /// <summary>
        /// retourne le chemin vers la frame courante
        /// </summary>
        string GetCurrentFramePath();

        /// <summary>
        /// Nom de l’animation actuellement jouée
        /// </summary>
        string CurrentAnimation { get; set; }

        void AddAnimation(string animationName, List<string> animationSprites);
    }
}

