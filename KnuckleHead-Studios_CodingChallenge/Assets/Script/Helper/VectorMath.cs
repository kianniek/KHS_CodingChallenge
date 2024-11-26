using UnityEngine;
using System.Collections;

namespace Helper
{
	//This is a static helper class that offers various methods for calculating and modifying vectors (as well as float values);
	public static class VectorMath {

		/// <summary>
		/// Calculate signed angle (ranging from -180 to +180) between '_vector_1' and '_vector_2';
		/// </summary>
		/// <param name="vector1"></param>
		/// <param name="vector2"></param>
		/// <param name="planeNormal"></param>
		/// <returns></returns>
		public static float GetAngle(Vector3 vector1, Vector3 vector2, Vector3 planeNormal)
		{
			//Calculate angle and sign;
			var angle = Vector3.Angle(vector1,vector2);
			var sign = Mathf.Sign(Vector3.Dot(planeNormal,Vector3.Cross(vector1,vector2)));
			
			//Combine angle and sign;
			var signedAngle = angle * sign;

			return signedAngle;
		}

		/// <summary>
		/// Returns the length of the part of a vector that points in the same direction as 'direction' (i.e., the dot product)
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static float GetDotProduct(Vector3 vector, Vector3 direction)
		{
			//Normalize vector if necessary;
			if (direction.sqrMagnitude != 1)
			{
				direction.Normalize();
			}
				
			var length = Vector3.Dot(vector, direction);

			return length;
		}
		
		/// <summary>
		/// Remove all parts from a vector that are pointing in the same direction as 'direction'
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3 RemoveDotVector(Vector3 vector, Vector3 direction)
		{
			//Normalize vector if necessary;
			if(direction.sqrMagnitude != 1)
				direction.Normalize();
			
			var amount = Vector3.Dot(vector, direction);
			
			vector -= direction * amount;
			
			return vector;
		}
		
		/// <summary>
		/// Extract and return parts from a vector that are pointing in the same direction as 'direction'
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3 ExtractDotVector(Vector3 vector, Vector3 direction)
		{
			//Normalize vector if necessary;
			if(direction.sqrMagnitude != 1)
				direction.Normalize();
			
			var amount = Vector3.Dot(vector, direction);
			
			return direction * amount;
		}

		/// <summary>
		/// Rotate a vector onto a plane defined by 'planeNormal'
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="planeNormal"></param>
		/// <param name="upDirection"></param>
		/// <returns></returns>
		public static Vector3 RotateVectorOntoPlane(Vector3 vector, Vector3 planeNormal, Vector3 upDirection)
		{
			//Calculate rotation;
			var rotation = Quaternion.FromToRotation(upDirection, planeNormal);

			//Apply rotation to vector;
			vector = rotation * vector;
			
			return vector;
		}

		/// <summary>
		/// Project a point onto a line defined by '_lineStartPosition' and '_lineDirection';
		/// </summary>
		/// <param name="lineStartPosition"></param>
		/// <param name="lineDirection"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static Vector3 ProjectPointOntoLine(Vector3 lineStartPosition, Vector3 lineDirection, Vector3 point)
		{		
			//Caclculate vector pointing from '_lineStartPosition' to '_point';
			var projectLine = point - lineStartPosition;
	
			var dotProduct = Vector3.Dot(projectLine, lineDirection);
	
			return lineStartPosition + lineDirection * dotProduct;
		}

		/// <summary>
		/// Increments a vector toward a target vector, using '_speed' and '_deltaTime';
		/// </summary>
		/// <param name="currentVector"></param>
		/// <param name="speed"></param>
		/// <param name="deltaTime"></param>
		/// <param name="targetVector"></param>
		/// <returns></returns>
		public static Vector3 IncrementVectorTowardTargetVector(Vector3 currentVector, float speed, float deltaTime, Vector3 targetVector)
		{
			return Vector3.MoveTowards(currentVector, targetVector, speed * deltaTime);
		}
	}
}
