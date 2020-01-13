import React, { Component } from 'component';
import Popup from '../../Popup/index';

export default function(WrappedComponent)
{
    return class WithPopupTextOverlay extends Component
    {
        ShowOverlayText(text, duration)
        {
            if (duration === undefined) duration = 3000;
        }

        render()
        {
            return (
                <div>
                    <WrappedComponent {...this.props} ShowOverlayText={this.ShowOverlayText} />
                </div>
            );
        }
    }
}