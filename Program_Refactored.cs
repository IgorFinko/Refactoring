namespace Cleanup
{
    internal class Program
    {
        private const double TargetChangeTime = 1;

        private double _currentTargetSetTime;
        private object? _lockedCandidateTarget;
        private object? _lockedTarget;
        private object? _activeTarget;
        private object? _currentTarget;
        private object? _targetInRangeContainer;

        public void CleanupTest(Frame frame)
        {
            ClearInvalidLockedTargets();

            TrySetActiveTargetFromQuantum(frame);

            if (IsCurrentTargetValid())
            {
                // No need to call SetCurrentTarget, as the target hasn't changed
                return;
            }

            var newTarget = GetLockedTarget() ?? GetActiveTarget() ?? _targetInRangeContainer?.GetTarget();

            if (newTarget != null)
            {
                SetCurrentTarget(newTarget);
            }
            else
            {
                ClearCurrentTarget();
            }
        }

        private void ClearInvalidLockedTargets()
        {
            if (_lockedCandidateTarget != null && !_lockedCandidateTarget.CanBeTarget())
            {
                _lockedCandidateTarget = null;
            }

            if (_lockedTarget != null && !_lockedTarget.CanBeTarget())
            {
                _lockedTarget = null;
            }
        }

        private bool IsCurrentTargetValid() => IsTargetValid(_currentTarget) && Time.time - _currentTargetSetTime < TargetChangeTime;

        private object? GetLockedTarget() => IsTargetValid(_lockedTarget) ? _lockedTarget : null;

        private object? GetActiveTarget() => IsTargetValid(_activeTarget) ? _activeTarget : null;

        private void SetCurrentTarget(object newTarget)
        {
            if (_currentTarget == newTarget)
            {
                return;
            }

            _currentTarget = newTarget;
            _currentTargetSetTime = Time.time;
            TargetableEntity.Selected = newTarget;
        }

        private void ClearCurrentTarget()
        {
            _currentTarget = null;
            TargetableEntity.Selected = null;
        }

        private static bool IsTargetValid(object? target) => target != null && target.CanBeTarget();
        
        private void TrySetActiveTargetFromQuantum(Frame frame)
        {
            throw new NotImplementedException();
        }
    }
}
