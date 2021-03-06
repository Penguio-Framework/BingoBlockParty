/*******************************************************************************
 * Copyright (c) 2013, Daniel Murphy
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 	* Redistributions of source code must retain the above copyright notice,
 * 	  this list of conditions and the following disclaimer.
 * 	* Redistributions in binary form must reproduce the above copyright notice,
 * 	  this list of conditions and the following disclaimer in the documentation
 * 	  and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 ******************************************************************************/
/**
 * Created at 12:11:41 PM Jan 23, 2011
 */


using org.jbox2d.common;

/**
 * Pulley joint definition. This requires two ground anchors, two dynamic body anchor points, and a
 * pulley ratio.
 * 
 * @author Daniel Murphy
 */

namespace org.jbox2d.dynamics.joints
{
    public class PulleyJointDef : JointDef
    {
        /**
   * The first ground anchor in world coordinates. This point never moves.
   */
        public Vec2 groundAnchorA;

        /**
   * The second ground anchor in world coordinates. This point never moves.
   */
        public Vec2 groundAnchorB;

        /**
   * The local anchor point relative to bodyA's origin.
   */

        /**
   * The a reference length for the segment attached to bodyA.
   */
        public double lengthA;

        /**
   * The a reference length for the segment attached to bodyB.
   */
        public double lengthB;
        public Vec2 localAnchorA;

        /**
   * The local anchor point relative to bodyB's origin.
   */
        public Vec2 localAnchorB;

        /**
   * The pulley ratio, used to simulate a block-and-tackle.
   */
        public double ratio;

        public PulleyJointDef()
        {
            type = JointType.PULLEY;
            groundAnchorA = new Vec2(-1.0d, 1.0d);
            groundAnchorB = new Vec2(1.0d, 1.0d);
            localAnchorA = new Vec2(-1.0d, 0.0d);
            localAnchorB = new Vec2(1.0d, 0.0d);
            lengthA = 0.0d;
            lengthB = 0.0d;
            ratio = 1.0d;
            collideConnected = true;
        }

        /**
   * Initialize the bodies, anchors, lengths, max lengths, and ratio using the world anchors.
   */

        public void initialize(Body b1, Body b2, Vec2 ga1, Vec2 ga2, Vec2 anchor1, Vec2 anchor2, double r)
        {
            bodyA = b1;
            bodyB = b2;
            groundAnchorA = ga1;
            groundAnchorB = ga2;
            localAnchorA = bodyA.getLocalPoint(anchor1);
            localAnchorB = bodyB.getLocalPoint(anchor2);
            Vec2 d1 = anchor1.sub(ga1);
            lengthA = d1.length();
            Vec2 d2 = anchor2.sub(ga2);
            lengthB = d2.length();
            ratio = r;
        }
    }
}