using System.Collections.Generic;
using UnityEngine;

namespace NullrefLib.Unity {
	public static class Detectors {
		#region 2D
		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 2D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="usesRight">When determining field of view, assign to 'true' if 'transform.right' is considered the front. Otherwise it will use 'transform.forward'.</param>
		/// <returns></returns>
		public static bool Detection2D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, bool usesRight = false) {

			Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, detectionRange, detectionLayerMask);
			if (cols != null && cols.Length > 0)
			{
				foreach (Collider2D item in cols)
				{
					Vector2 relativePos = item.transform.position - transform.position;
					if (Vector2.Angle(usesRight ? transform.right : transform.forward, relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit2D hit = Physics2D.Raycast(transform.position, relativePos, detectionRange,
							detectionSightlineLayerMask);
						if (detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							target = hit.transform;
							return true;
						}
					}
				}
			}
			target = null;
			return false;
		}
		#endregion

		#region 3D
		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="usesRight">When determining field of view, assign to 'true' if 'transform.right' is considered the front. Otherwise it will use 'transform.forward'.</param>
		/// <returns></returns>
		public static bool Detection3D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, bool usesRight = false,
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore) {
			return Detection3D(transform, detectionLayerMask, detectionRange, detectionFOV,
				detectionSightlineLayerMask, out target, Vector3.zero, usesRight, queryTriggerInteraction);
		}

		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="deltaPos">View offset for when the "eyes" are not located at the object pivot</param>
		/// <param name="usesRight">When determining field of view, assign to 'true' if 'transform.right' is considered the front. Otherwise it will use 'transform.forward'.</param>
		/// <returns></returns>
		public static bool Detection3D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, Vector3 deltaPos,
			bool usesRight = false, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore) {
			return Detection3D(transform, detectionLayerMask, detectionRange, detectionFOV,
				 detectionSightlineLayerMask, out target, deltaPos, usesRight ? Vector3.right : Vector3.forward, queryTriggerInteraction);
		}


		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="deltaPos">View offset for when the "eyes" are not located at the object pivot</param>
		/// <param name="frontDirection">Determines the direction considered as "front", relative to the transform.
		/// It will be rotated accordingly to the transform's rotation.</param>
		/// <returns></returns>
		public static bool Detection3D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, Vector3 deltaPos,
			Vector3 frontDirection, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore) {
			Vector3 refPos = transform.position + (transform.rotation * deltaPos);
			Collider[] cols = Physics.OverlapSphere(refPos, detectionRange, detectionLayerMask, queryTriggerInteraction);
			if (cols != null && cols.Length > 0)
			{
				foreach (Collider item in cols)
				{
					Vector3 relativePos = item.transform.position - refPos;
					if (Vector3.Angle(transform.rotation * frontDirection, relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit hit;
						bool didHit = Physics.Raycast(refPos, relativePos, out hit, detectionRange,
							detectionSightlineLayerMask, queryTriggerInteraction);
						if (didHit && detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							target = hit.transform;
							return true;
						}
					}
				}
			}
			target = null;
			return false;
		}

		#endregion

		#region 3D All
		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectableLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="lineOfSightOpaquenessMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="colliderCache">A reusable cache used for an OverlapSphere</param>
		/// <param name="targetsCache">Reusable list that will contain found targets</param>
		/// <param name="deltaPos">View offset for when the "eyes" are not located at the object pivot</param>
		/// <param name="forward">The front of the detection cone</param>
		/// <returns>True if any target was found</returns>
		public static bool Detection3DAll(Transform transform, LayerMask detectableLayerMask,
			float detectionRange, float detectionFOV, LayerMask lineOfSightOpaquenessMask,
			List<Transform> targetsCache, Vector3 deltaPos, Collider[] colliderCache, Vector3 forward,
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore) {
			const float raycastSourceOverlapCheckSize = 0.025f; // Tiny check size const

			targetsCache.Clear();

			Vector3 refPos = transform.position + (transform.rotation * deltaPos); // Get the center/origin point for all checks.

			Physics.OverlapSphereNonAlloc(refPos, raycastSourceOverlapCheckSize,
				colliderCache, detectableLayerMask, queryTriggerInteraction); // Tiny check to get collider inside player's head.
			Collider insideOriginCollider = colliderCache[0]; // Only need one.

			// Actual full radius check.
			int c = Physics.OverlapSphereNonAlloc(refPos, detectionRange, colliderCache, detectableLayerMask, queryTriggerInteraction);

			bool found = false;

			for (int i = 0; i < c; i++)
			{
				Collider current = colliderCache[i]; // Cache current element

				Vector3 relativePos = current.transform.position - transform.position; // Relative position to target.
				if (Vector3.Angle(forward, relativePos) < detectionFOV * 0.5f) // Check it's inside FOV.
				{

					// Raycast will not hit element if origin is inside the collider. Use previous tiny check instead.
					if (insideOriginCollider == current && detectableLayerMask.ContainsLayer(current.gameObject.layer))
					{
						found = true;
						targetsCache.Add(current.transform);
						continue;
					}

					RaycastHit hit;

					bool didHit = Physics.Raycast(refPos, relativePos, out hit, detectionRange,
						lineOfSightOpaquenessMask, queryTriggerInteraction); // Raycast line of sight to element.
					if (didHit && hit.collider == current && detectableLayerMask.ContainsLayer(hit.collider.gameObject.layer)) // If there's line of sight, add.
					{
						targetsCache.Add(hit.transform);
						found = true;
					}
				}
			}
			return found; // Return true if any was found.
		}

		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectableLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="lineOfSightOpaquenessMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="colliderCache">A reusable cache used for an OverlapSphere</param>
		/// <param name="targetsCache">Reusable list that will contain found targets</param>
		/// <param name="forward">The front of the detection cone</param>
		/// <returns>True if any target was found</returns>
		public static bool Detection3DAll(Transform transform, LayerMask detectableLayerMask, float detectionRange,
			float detectionFOV, LayerMask lineOfSightOpaquenessMask, List<Transform> targetsCache, Collider[] colliderCache,
			Vector3 forward, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore) {
			return Detection3DAll(transform, detectableLayerMask, detectionRange, detectionFOV, lineOfSightOpaquenessMask,
				targetsCache, Vector3.zero, colliderCache, forward, queryTriggerInteraction);
		}
		#endregion
	}
}