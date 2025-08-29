using UnityEngine;

public class HapticFeedback : MonoBehaviour
{
    private void Awake()
    {
        RangeWeapon.OnBulletShoot += Vibrate;
        BumpyButton.OnButtonPressed += VibrateMedium;
    }

    private void OnDestroy()
    {
        RangeWeapon.OnBulletShoot -= Vibrate;
        BumpyButton.OnButtonPressed -= VibrateMedium;


    }

    private void Vibrate()
    {
        CandyCoded.HapticFeedback.HapticFeedback.LightFeedback();
    }

    private void VibrateMedium()
    {
        CandyCoded.HapticFeedback.HapticFeedback.MediumFeedback();

    }
}
