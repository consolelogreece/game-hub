import React from 'react';
import './styles.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChessBishop, faChessRook, faChessPawn, faChessKnight } from '@fortawesome/free-solid-svg-icons'

export default props => 
{
    return (
        <div className={`loading ${!props.render  ? "loading-fade-out" : ""}`}>
            <FontAwesomeIcon icon={faChessRook} className="loading-element loading-rook" />
            <FontAwesomeIcon icon={faChessKnight} className="loading-element loading-knight" />
            <FontAwesomeIcon icon={faChessBishop} className="loading-element loading-bishop" />
            <FontAwesomeIcon icon={faChessPawn} className="loading-element loading-pawn" />
        </div>
    )
}