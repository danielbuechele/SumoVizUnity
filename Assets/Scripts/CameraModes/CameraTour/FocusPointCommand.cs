using UnityEngine;
using System.Collections;

public class FocusPointCommand {

	private bool changeFocus;
	private Vector3 focusPoint;


	public FocusPointCommand() {
		this.changeFocus = false;
	}
		
	public FocusPointCommand(Vector3 focusPoint) {
		this.focusPoint = focusPoint;
		this.changeFocus = true;
	}

	public bool doChangeFocus() {
		return changeFocus;
	}

	public Vector3 getFocusPoint() {
		return focusPoint;
	}

}
