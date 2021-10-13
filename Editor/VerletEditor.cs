using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

[CustomEditor(typeof(VerletSimulation))]
public class VerletEditor : Editor
{

    Color selectedColor = Color.blue;
    Color unselectedColor = Color.gray;
    Color highLightedColor = Color.yellow;
    Color lockedColor = Color.red;

    [SerializeField] private VerletSimulation verletSimulation;
    [SerializeField] private VerletManager manager;

    private bool selectionPossible = false;
    private int selectedPoint1 = -1;
    private int selectedPoint2 = -1;

    private bool stickMode = false;
    private bool leftClicked = false;
    private bool rightClicked = false;

    // Draw handles
    private void Draw() {

        if(manager != null) {

            if(Event.current.type == EventType.KeyDown && Event.current.keyCode == (KeyCode.LeftShift)) stickMode = true;
            else if (Event.current.type == EventType.KeyUp && Event.current.keyCode == (KeyCode.LeftShift)) stickMode = false;

            leftClicked = Event.current.type == EventType.MouseDown && Event.current.button == 0;
            rightClicked = Event.current.type == EventType.MouseDown && Event.current.button == 1;

            for (int i = 0; i < manager.sticks.Count; i++) {

                if(manager.sticks[i] == null) continue;

                Vector3 center = (manager.sticks[i].pointA.Position + manager.sticks[i].pointB.Position) / 2;

                if (HandleUtility.DistanceToCircle( center, 0.1f ) < 5) {
                    if (leftClicked) {
                        manager.sticks.Remove( manager.sticks[i] );
                    }
                }

                Handles.DrawDottedLine( manager.sticks[i].pointA.Position, manager.sticks[i].pointB.Position, 3.5f );

            }

            for (int i = 0; i < manager.points.Count; i++) {

                if (manager.points[i] == null) continue;

                // Change color if selected
                if(selectedPoint1 == i || selectedPoint2 == i) {
                    Handles.color = selectedColor;
                } else {
                    if(manager.points[i].locked) {
                        Handles.color = lockedColor;
                    }else{
                        Handles.color = unselectedColor;    
                    }
                }

                Vector3 editedPosition = Handles.FreeMoveHandle(
                    manager.points[i].Position,
                    Quaternion.identity,
                    0.17f,
                    Vector3.zero,
                    Handles.CylinderHandleCap
                );

                Handles.Label( manager.points[i].Position, $"Point {i}" );

                if(!stickMode) {

                    manager.points[i].Position = editedPosition;

                    if(leftClicked) DeselectAll();

                }else{

                    if( HandleUtility.DistanceToCircle(manager.points[i].Position, 0.17f) < 2) {

                        Handles.color = highLightedColor;

                        if(leftClicked){
                            SelectPoint( i );
                        }

                        if(rightClicked){
                            manager.points[i].locked = !manager.points[i].locked;
                        }

                    }

                }

            }

        }else{

            Debug.LogWarning( "Adquiring reference to the manager..." );
            verletSimulation = target as VerletSimulation;
            manager = verletSimulation.manager;

        }

    }

    private void SelectPoint(int index) {


        if(selectedPoint1 == -1) {

            selectedPoint1 = index;
            Debug.Log( $"Selected point: {index}" );

        } else {

            if(index != selectedPoint1) {
                selectedPoint2 = index;
                manager.CreateStick( selectedPoint1, selectedPoint2 );
                DeselectAll();
            }else{
                DeselectAll();
            }

        }

    }

    private void CreateStick(int index1, int index2){
        
    }

    private void DeselectAll() {
        Debug.Log( "Selection cancelled." );
        selectedPoint1 = -1;
        selectedPoint2 = -1;
    }

    private void OnSceneGUI() {
        Draw();
    }

}
