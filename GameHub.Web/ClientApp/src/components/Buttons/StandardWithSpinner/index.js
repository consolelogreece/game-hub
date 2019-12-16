import React from 'react';
import Button from '../Standard';
import LoadingSpinner from '../../../components/LoadingSpinner';
import './styles.scss';

export default props => 
{
    let submitButtonStyles = {color: "white", margin: "0 auto"};

    if (props.loadingStatus !== "none")
    {
        submitButtonStyles.color = "rgba(0,0,0,0)";
    }

    let properinos = {...props, style: {...props.style, ...submitButtonStyles}};

    console.log(properinos)

    return (
        <div>
            {props.loadingStatus !== "none" && (
                <div className={"button-loading-spinner-container"}>
                    <div className={"button-loading-spinner"}>
                        <LoadingSpinner status={props.loadingStatus} />
                    </div>
                </div>
            )}
            <Button {...properinos}>Join</Button>
        </div>
    );
}