
export let CalculateRelativePositon = (elBoundingRect,x,y) =>
{
    let gridLeft = elBoundingRect.left;
    let gridTop = elBoundingRect.top;

    let relativeX = x - gridLeft;
    let relativeY = y - gridTop;

    return {row: relativeY, col: relativeX};
}