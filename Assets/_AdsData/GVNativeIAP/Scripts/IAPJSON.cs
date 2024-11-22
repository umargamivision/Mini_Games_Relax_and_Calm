using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GVNativeIAP{
	[System.Serializable]
	public class IAPRestoreIDs{
		public int status=-1;
		public int count=0;
		public string[] productIds;

		public override string ToString(){
			string temp = "";
			temp += "status: " + status;
			temp += " count: " + count;
			temp += " productIds: ";
			if (productIds != null) {
				for (int i = 0; i < productIds.Length; i++)
					temp += " [" + i + "] " + productIds [i] + " : ";
			}
			return temp;
		}
	}
}