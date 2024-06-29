using System;
using System.Globalization;
using UnityEngine;

// compile with: -doc:DocFileName.xml 
namespace NullrefLib.Unity {

	public static class MiscUnityUtilities {
		public static Quaternion Slerp2D(Vector2 a, Vector2 b, float t, bool usesRight = false) {

			float fa;
			float fb;

			if (!usesRight)
			{
				fa = Vector2.SignedAngle(Vector2.up, a);
				fb = Vector2.SignedAngle(Vector2.up, b);
			} else
			{
				fa = Vector2.SignedAngle(Vector2.right, a);
				fb = Vector2.SignedAngle(Vector2.right, b);
			}

			Quaternion qa = Quaternion.AngleAxis(fa, Vector3.forward);
			Quaternion qb = Quaternion.AngleAxis(fb, Vector3.forward);

			return Quaternion.Slerp(qa, qb, t);
		}

		public static Quaternion SlerpWithAxises(Vector3 a, Vector3 b, float t, Vector3 reference, Vector3 axis) {

			float fa = Vector3.SignedAngle(reference, a, axis);
			float fb = Vector3.SignedAngle(reference, b, axis);

			Quaternion qa = Quaternion.AngleAxis(fa, axis);
			Quaternion qb = Quaternion.AngleAxis(fb, axis);

			return Quaternion.Slerp(qa, qb, t);
		}

		public static Vector3 Vector3XY(Vector2 vec, float z) => new Vector3(vec.x, vec.y, z);

		public static Vector3 Vector3XZ(Vector2 vec, float y) => new Vector3(vec.x, y, vec.y);

		/// <summary>
		/// Returns true if the angle between vec1 and vec2 is smaller than the given angle.
		/// </summary>
		/// <param name="vec1">First vector</param>
		/// <param name="vec2">Second vector</param>
		/// <param name="comparisonAngle">Angle to compare against in degrees.</param>
		/// <returns></returns>
		public static bool AngleBetweenIsSmaller(Vector3 vec1, Vector3 vec2, float comparisonAngle)
			=> Vector3.Angle(vec1, vec2) < comparisonAngle;

		/// <summary>
		/// Returns Cosine of angle. Listen, it may be easy, but sometimes you need a reminder
		/// </summary>
		public static float DotValueForNormalizedAngles(float degrees) => Mathf.Cos(degrees);

		/// <summary>
		/// Given an int value, returns a layer mask for that layer alone.
		/// </summary>
		/// <param name="layer">Layer to create a layer mask from (0-31)</param>
		/// <returns></returns>
		public static LayerMask IntToLayerMask(int layer) {
			return 1 << layer;
		}

		/// <summary>
		/// Converts a hex code into corresponding color. Supports RGB and RGBA
		/// </summary>
		/// <param name="hex">Color hex code, without prefixes.</param>
		/// <returns></returns>
		public static Color ParseColor(string hex) {
			int length = hex.Length;
			if (!(length == 6 || length == 8))
				throw new ArgumentException($"Color Hex code {hex} is not a valid hex code.");

			Color32 color = new();
			if (
				byte.TryParse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier, NumberFormatInfo.InvariantInfo, out byte r) &&
				byte.TryParse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier, NumberFormatInfo.InvariantInfo, out byte g) &&
				byte.TryParse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier, NumberFormatInfo.InvariantInfo, out byte b))
			{
				color.r = r;
				color.b = b;
				color.g = g;
			} else
				throw new ArgumentException($"Color Hex code {hex} is not a valid hex code.");


			if (length == 8)
			{
				if (byte.TryParse(hex.Substring(6, 2), NumberStyles.AllowHexSpecifier, NumberFormatInfo.InvariantInfo, out byte a))
					color.a = a;
				else
					throw new ArgumentException($"Color Hex code {hex} is not a valid hex code.");

			} else
				color.a = 0xFF;

			return color;
		}

		public static void Swap(ref float v1, ref float v2) {
			(v2, v1) = (v1, v2);
		}
	}
}
