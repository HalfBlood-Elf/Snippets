using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace Argentics.Package.Cinemachine.Extentions
{
	public static class CinemachineFramingTransposerExt
	{
		/// <summary>
		/// Copies visible inspector parameters from other <see cref="CinemachineFramingTransposer"/>
		/// </summary>
		/// <param name="self"></param>
		/// <param name="other"></param>
		public static void CopyInspectorFieldsOf(this CinemachineFramingTransposer self, CinemachineFramingTransposer other)
		{
			self.m_TrackedObjectOffset = other.m_TrackedObjectOffset;

			self.m_LookaheadTime = other.m_LookaheadTime;
			self.m_LookaheadSmoothing = other.m_LookaheadSmoothing;
			self.m_LookaheadIgnoreY = other.m_LookaheadIgnoreY;

			self.m_XDamping = other.m_XDamping;
			self.m_YDamping = other.m_YDamping;
			self.m_ZDamping = other.m_ZDamping;
			self.m_TargetMovementOnly = other.m_TargetMovementOnly;

			self.m_ScreenX = other.m_ScreenX;
			self.m_ScreenY = other.m_ScreenY;
			self.m_CameraDistance = other.m_CameraDistance;

			self.m_DeadZoneWidth = other.m_DeadZoneWidth;
			self.m_DeadZoneHeight = other.m_DeadZoneHeight;
			self.m_DeadZoneDepth = other.m_DeadZoneDepth;

			self.m_UnlimitedSoftZone = other.m_UnlimitedSoftZone;
			self.m_SoftZoneWidth = other.m_SoftZoneWidth;
			self.m_SoftZoneHeight = other.m_SoftZoneHeight;

			self.m_BiasY = other.m_BiasY;
			self.m_BiasX = other.m_BiasX;
			self.m_CenterOnActivate = other.m_CenterOnActivate;


			self.m_GroupFramingMode = other.m_GroupFramingMode;
			self.m_GroupFramingSize = other.m_GroupFramingSize;

			// next are CinemachineFramingTransposer fields, but excluded from inspector in prespective camera

			self.m_MaximumOrthoSize = other.m_MaximumOrthoSize;
			self.m_MinimumOrthoSize = other.m_MinimumOrthoSize;

			self.m_AdjustmentMode = other.m_AdjustmentMode;
			self.m_MaximumFOV = other.m_MaximumFOV;
			self.m_MinimumFOV = other.m_MinimumFOV;

			self.m_MaximumDistance = other.m_MaximumDistance;
			self.m_MinimumDistance = other.m_MinimumDistance;
			self.m_MaxDollyOut = other.m_MaxDollyOut;
			self.m_MaxDollyIn = other.m_MaxDollyIn;


		}
	}
}