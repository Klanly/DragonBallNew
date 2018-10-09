using UnityEngine;
using System.Collections;

public class RenderQueueChange : MonoBehaviour {

	public enum RenderType {
		FRONT,
		BACK
	}
	
	public RenderType m_type = RenderType.FRONT;

	private Renderer mainRenderer;

	private int _lastQueue = 0;
	
	void Start() {
		mainRenderer = this.renderer;
		changeRenderQueue();
	}
	
	void changeRenderQueue() {
		int queue = mainRenderer.material.renderQueue;
		queue += m_type == RenderType.FRONT ? 1 : -1;

		if (_lastQueue != queue) {
			_lastQueue = queue;
			
			mainRenderer.material.renderQueue = _lastQueue;
		}
	}

}
