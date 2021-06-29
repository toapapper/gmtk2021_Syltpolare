using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor.Playables;
using UnityEditor.Timeline;
using UnityEditor;

namespace Celezt.Timeline
{
    [CustomTimelineEditor(typeof(AnnotationMarker)), CanEditMultipleObjects]
    public class AnnotationMarkerEditor : MarkerEditor
    {
        private const float _lineOverlayWidth = 6.0f;

        private const string _annotationPath = "annotation";
        private const string _annotationCollapsedPath = "annotation_collapsed";

        private static Texture2D _annotationTexture;
        private static Texture2D _annotationCollapsedTexture;

        static AnnotationMarkerEditor()
        {
            _annotationTexture = Resources.Load<Texture2D>(_annotationPath);
            _annotationCollapsedTexture = Resources.Load<Texture2D>(_annotationCollapsedPath);
        }

        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            AnnotationMarker annotation = marker as AnnotationMarker;

            if (annotation != null)
                return new MarkerDrawOptions { tooltip = annotation.Note };

            return base.GetMarkerOptions(marker);
        }

        public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
        {
            AnnotationMarker annotation = marker as AnnotationMarker;

            if (annotation == null)
                return;

            if (annotation.ShowOverlay)
                DrawLineOverlay(region, annotation.Color);

            DrawColorOverlay(region, annotation.Color, uiState);
        }

        private static void DrawLineOverlay(MarkerOverlayRegion region, Color color)
        {
            // Calculate markerRegion's center on the x axis.
            float markerRegionCenter = region.markerRegion.xMin + (region.markerRegion.width - _lineOverlayWidth) / 2.0f;

            // Calculate a rectangle that uses the full timeline region's height.
            Rect lineRect = new Rect
            {
                x = markerRegionCenter,
                y = region.timelineRegion.y,
                width = _lineOverlayWidth,
                height = region.timelineRegion.height,
            };

            Color lineColor = new Color(color.r, color.g, color.b, color.a * 0.4f);
            EditorGUI.DrawRect(lineRect, lineColor);
        }

        private static void DrawColorOverlay(MarkerOverlayRegion region, Color color, MarkerUIStates state)
        {
            // Save the Editor's overlay color before changing it.
            Color oldColor = GUI.color;
            GUI.color = color;

            if (state.HasFlag(MarkerUIStates.Selected))
            {
                GUI.DrawTexture(region.markerRegion, _annotationTexture);
            }
            else if (state.HasFlag(MarkerUIStates.Collapsed))
            {
                GUI.DrawTexture(region.markerRegion, _annotationCollapsedTexture);
            }
            else if (state.HasFlag(MarkerUIStates.None))
            {
                GUI.DrawTexture(region.markerRegion, _annotationTexture);
            }

            // Restore the previous Editor's overlay color.
            GUI.color = oldColor;
        }
    }
}