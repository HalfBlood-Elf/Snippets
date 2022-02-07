using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static Cinemachine.CinemachineFramingTransposer;


namespace Argentics.Package.Cinemachine.Extentions
{
	//[CreateAssetMenu(menuName = "Argentics/Packages/Cinemachine/Virtual Camera/CinemachineFramingTransposer Settings")]
	[System.Serializable]
	public class CinemachineFramingTransposerSettings
	{
		/// <summary>
		/// Offset from the Follow Target object (in target-local co-ordinates).  The camera will attempt to
		/// frame the point which is the target's position plus this offset.  Use it to correct for
		/// cases when the target's origin is not the point of interest for the camera.
		/// </summary>
		[Tooltip("Offset from the Follow Target object (in target-local co-ordinates).  "
			+ "The camera will attempt to frame the point which is the target's position plus "
			+ "this offset.  Use it to correct for cases when the target's origin is not the "
			+ "point of interest for the camera.")]
		public Vector3 m_TrackedObjectOffset;

		/// <summary>This setting will instruct the composer to adjust its target offset based
		/// on the motion of the target.  The composer will look at a point where it estimates
		/// the target will be this many seconds into the future.  Note that this setting is sensitive
		/// to noisy animation, and can amplify the noise, resulting in undesirable camera jitter.
		/// If the camera jitters unacceptably when the target is in motion, turn down this setting,
		/// or animate the target more smoothly.</summary>
		[Tooltip("This setting will instruct the composer to adjust its target offset based on the "
			+ "motion of the target.  The composer will look at a point where it estimates the target "
			+ "will be this many seconds into the future.  Note that this setting is sensitive to noisy "
			+ "animation, and can amplify the noise, resulting in undesirable camera jitter.  "
			+ "If the camera jitters unacceptably when the target is in motion, turn down this "
			+ "setting, or animate the target more smoothly.")]
		[Range(0f, 1f)]
		[Space]
		public float m_LookaheadTime = 0;

		/// <summary>Controls the smoothness of the lookahead algorithm.  Larger values smooth out
		/// jittery predictions and also increase prediction lag</summary>
		[Tooltip("Controls the smoothness of the lookahead algorithm.  Larger values smooth out "
			+ "jittery predictions and also increase prediction lag")]
		[Range(0, 30)]
		public float m_LookaheadSmoothing = 0;

		/// <summary>If checked, movement along the Y axis will be ignored for lookahead calculations</summary>
		[Tooltip("If checked, movement along the Y axis will be ignored for lookahead calculations")]
		public bool m_LookaheadIgnoreY;

		/// <summary>How aggressively the camera tries to maintain the offset in the X-axis.
		/// Small numbers are more responsive, rapidly translating the camera to keep the target's
		/// x-axis offset.  Larger numbers give a more heavy slowly responding camera.
		/// Using different settings per axis can yield a wide range of camera behaviors</summary>
		[Space]
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the X-axis.  "
			+ "Small numbers are more responsive, rapidly translating the camera to keep the target's "
			+ "x-axis offset.  Larger numbers give a more heavy slowly responding camera.  "
			+ "Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_XDamping = 1f;

		/// <summary>How aggressively the camera tries to maintain the offset in the Y-axis.
		/// Small numbers are more responsive, rapidly translating the camera to keep the target's
		/// y-axis offset.  Larger numbers give a more heavy slowly responding camera.
		/// Using different settings per axis can yield a wide range of camera behaviors</summary>
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the Y-axis.  "
			+ "Small numbers are more responsive, rapidly translating the camera to keep the target's "
			+ "y-axis offset.  Larger numbers give a more heavy slowly responding camera.  "
			+ "Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_YDamping = 1f;

		/// <summary>How aggressively the camera tries to maintain the offset in the Z-axis.
		/// Small numbers are more responsive, rapidly translating the camera to keep the
		/// target's z-axis offset.  Larger numbers give a more heavy slowly responding camera.
		/// Using different settings per axis can yield a wide range of camera behaviors</summary>
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the Z-axis.  "
			+ "Small numbers are more responsive, rapidly translating the camera to keep the target's "
			+ "z-axis offset.  Larger numbers give a more heavy slowly responding camera.  "
			+ "Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_ZDamping = 1f;

		/// <summary>If set, damping will apply only to target motion, and not when 
		/// the camera rotation changes.  Turn this on to get an instant response when 
		/// the rotation changes</summary>
		[Tooltip("If set, damping will apply  only to target motion, but not to camera "
			+ "rotation changes.  Turn this on to get an instant response when the rotation changes.  ")]
		public bool m_TargetMovementOnly = true;

		/// <summary>Horizontal screen position for target. The camera will move to position the tracked object here</summary>
		[Space]
		[Range(-0.5f, 1.5f)]
		[Tooltip("Horizontal screen position for target. The camera will move to position the tracked object here.")]
		public float m_ScreenX = 0.5f;

		/// <summary>Vertical screen position for target, The camera will move to to position the tracked object here</summary>
		[Range(-0.5f, 1.5f)]
		[Tooltip("Vertical screen position for target, The camera will move to position the tracked object here.")]
		public float m_ScreenY = 0.5f;

		/// <summary>The distance along the camera axis that will be maintained from the Follow target</summary>
		[Tooltip("The distance along the camera axis that will be maintained from the Follow target")]
		public float m_CameraDistance = 10f;

		/// <summary>Camera will not move horizontally if the target is within this range of the position</summary>
		[Space]
		[Range(0f, 2f)]
		[Tooltip("Camera will not move horizontally if the target is within this range of the position.")]
		public float m_DeadZoneWidth = 0f;

		/// <summary>Camera will not move vertically if the target is within this range of the position</summary>
		[Range(0f, 2f)]
		[Tooltip("Camera will not move vertically if the target is within this range of the position.")]
		public float m_DeadZoneHeight = 0f;

		/// <summary>The camera will not move along its z-axis if the Follow target is within
		/// this distance of the specified camera distance</summary>
		[Tooltip("The camera will not move along its z-axis if the Follow target is within "
			+ "this distance of the specified camera distance")]
		public float m_DeadZoneDepth = 0;

		/// <summary>If checked, then then soft zone will be unlimited in size</summary>
		[Space]
		[Tooltip("If checked, then then soft zone will be unlimited in size.")]
		public bool m_UnlimitedSoftZone = false;

		/// <summary>When target is within this region, camera will gradually move to re-align
		/// towards the desired position, depending onm the damping speed</summary>
		[Range(0f, 2f)]
		[Tooltip("When target is within this region, camera will gradually move horizontally to "
			+ "re-align towards the desired position, depending on the damping speed.")]
		public float m_SoftZoneWidth = 0.8f;

		/// <summary>When target is within this region, camera will gradually move to re-align
		/// towards the desired position, depending onm the damping speed</summary>
		[Range(0f, 2f)]
		[Tooltip("When target is within this region, camera will gradually move vertically to "
			+ "re-align towards the desired position, depending on the damping speed.")]
		public float m_SoftZoneHeight = 0.8f;

		/// <summary>A non-zero bias will move the targt position away from the center of the soft zone</summary>
		[Range(-0.5f, 0.5f)]
		[Tooltip("A non-zero bias will move the target position horizontally away from the center of the soft zone.")]
		public float m_BiasX = 0f;

		/// <summary>A non-zero bias will move the targt position away from the center of the soft zone</summary>
		[Range(-0.5f, 0.5f)]
		[Tooltip("A non-zero bias will move the target position vertically away from the center of the soft zone.")]
		public float m_BiasY = 0f;

		/// <summary>Force target to center of screen when this camera activates.
		/// If false, will clamp target to the edges of the dead zone</summary>
		[Tooltip("Force target to center of screen when this camera activates.  "
			+ "If false, will clamp target to the edges of the dead zone")]
		public bool m_CenterOnActivate = true;


		[Space]
		[Tooltip("What screen dimensions to consider when framing.  Can be Horizontal, Vertical, or both")]
		public FramingMode m_GroupFramingMode = FramingMode.HorizontalAndVertical;

		/// <summary>How to adjust the camera to get the desired framing</summary>
		[Tooltip("How to adjust the camera to get the desired framing.  You can zoom, dolly in/out, or do both.")]
		public AdjustmentMode m_AdjustmentMode = AdjustmentMode.ZoomOnly;

		/// <summary>How much of the screen to fill with the bounding box of the targets.</summary>
		[Tooltip("The bounding box of the targets should occupy this amount of the screen space.  "
			+ "1 means fill the whole screen.  0.5 means fill half the screen, etc.")]
		public float m_GroupFramingSize = 0.8f;

		/// <summary>How much closer to the target can the camera go?</summary>
		[Tooltip("The maximum distance toward the target that this behaviour is allowed to move the camera.")]
		public float m_MaxDollyIn = 5000f;

		/// <summary>How much farther from the target can the camera go?</summary>
		[Tooltip("The maximum distance away the target that this behaviour is allowed to move the camera.")]
		public float m_MaxDollyOut = 5000f;

		/// <summary>Set this to limit how close to the target the camera can get</summary>
		[Tooltip("Set this to limit how close to the target the camera can get.")]
		public float m_MinimumDistance = 1;

		/// <summary>Set this to limit how far from the taregt the camera can get</summary>
		[Tooltip("Set this to limit how far from the target the camera can get.")]
		public float m_MaximumDistance = 5000f;

		/// <summary>If adjusting FOV, will not set the FOV lower than this</summary>
		[Range(1, 179)]
		[Tooltip("If adjusting FOV, will not set the FOV lower than this.")]
		public float m_MinimumFOV = 3;

		/// <summary>If adjusting FOV, will not set the FOV higher than this</summary>
		[Range(1, 179)]
		[Tooltip("If adjusting FOV, will not set the FOV higher than this.")]
		public float m_MaximumFOV = 60;

		/// <summary>If adjusting Orthographic Size, will not set it lower than this</summary>
		[Tooltip("If adjusting Orthographic Size, will not set it lower than this.")]
		public float m_MinimumOrthoSize = 1;

		/// <summary>If adjusting Orthographic Size, will not set it higher than this</summary>
		[Tooltip("If adjusting Orthographic Size, will not set it higher than this.")]
		public float m_MaximumOrthoSize = 5000;

		public void ApplyTo(CinemachineFramingTransposer framingTransposer)
		{
			framingTransposer.m_TrackedObjectOffset = m_TrackedObjectOffset;

			framingTransposer.m_LookaheadTime = m_LookaheadTime;
			framingTransposer.m_LookaheadSmoothing = m_LookaheadSmoothing;
			framingTransposer.m_LookaheadIgnoreY = m_LookaheadIgnoreY;

			framingTransposer.m_XDamping = m_XDamping;
			framingTransposer.m_YDamping = m_YDamping;
			framingTransposer.m_ZDamping = m_ZDamping;
			framingTransposer.m_TargetMovementOnly = m_TargetMovementOnly;

			framingTransposer.m_ScreenX = m_ScreenX;
			framingTransposer.m_ScreenY = m_ScreenY;
			framingTransposer.m_CameraDistance = m_CameraDistance;

			framingTransposer.m_DeadZoneWidth = m_DeadZoneWidth;
			framingTransposer.m_DeadZoneHeight = m_DeadZoneHeight;
			framingTransposer.m_DeadZoneDepth = m_DeadZoneDepth;

			framingTransposer.m_UnlimitedSoftZone = m_UnlimitedSoftZone;
			framingTransposer.m_SoftZoneWidth = m_SoftZoneWidth;
			framingTransposer.m_SoftZoneHeight = m_SoftZoneHeight;
			framingTransposer.m_BiasY = m_BiasY;
			framingTransposer.m_BiasX = m_BiasX;
			framingTransposer.m_CenterOnActivate = m_CenterOnActivate;
		}

		public void CopyFrom(CinemachineFramingTransposer framingTransposer)
		{
		}
	}



}

